// using Finlay.PharmaVigilance.Application.DTO;
// using Finlay.PharmaVigilance.Application.IServices;
// using Finlay.PharmaVigilance.Domain.Enum;
// using Finlay.PharmaVigilance.Domain.Events;
// using MassTransit;


// namespace Finlay.PharmaVigilance.Infrastructure.Consumers;

// public class MedicalReviewerConsumer : IConsumer<MedicalReviewerAssignmentEvent>
// {
//     private readonly IEmailService _emailService;

//     public MedicalReviewerConsumer(IEmailService emailService)
//     {
//         _emailService = emailService;
//     }

//     public async Task Consume(ConsumeContext<MedicalReviewerRegisteredEvent> context)
//     {
//         var data = context.Message;

//         var url =
//             $"http://localhost:5173/activate-account" +
//             $"?email={data.Email}" +
//             $"&token={Uri.EscapeDataString(data.Token)}";

//         await _emailService.SendEmailAsync(
//             data.Email!,
//             EmailTemplateType.ActivateAccount,
//             new ActivateAccountTemplate
//             {
//                 FullName = data.FullName,
//                 ActivationUrl = url
//             }
//         );

//     }

// }