using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Domain.Events;
using MassTransit;


namespace Finlay.PharmaVigilance.Infrastructure.Consumers;

public class MedicalReviewerConsumer : IConsumer<MedicalReviewerRegisteredEvent>
{
    private readonly IEmailService _emailService;

    public MedicalReviewerConsumer(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task Consume(ConsumeContext<MedicalReviewerRegisteredEvent> context)
    {
        var data = context.Message;

        // await _emailService.SendEmailAsync(
        //     data.Email,
        //     "Welcome",
        //     $"Hola {data.Email}, bienvenido"
        // );

        var token = data.Token;

        Console.WriteLine(token);

        var url =
            $"https://frontend-five-sepia-10.vercel.app/activate-account" +
            $"?email={data.Email}" +
            $"&token={Uri.EscapeDataString(token)}";

        await _emailService.SendEmailAsync(
            data.Email!,
            "Activación de cuenta",
            $"Active su cuenta aquí: {url}"
        );

    }

}