using LMS___Mini_Version.Features.Enrollments.Commands;
using LMS___Mini_Version.Services.Interfaces;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Handlers
{
    public class TransferEnrollmentCommandHandler : IRequestHandler<TransferEnrollmentCommand>
    {
        private readonly ITrackService _trackService;
        private readonly IEnrollmentService _enrollmentService;
        private readonly IPaymentService _paymentService;

        public TransferEnrollmentCommandHandler(ITrackService trackService,IEnrollmentService enrollmentService, IPaymentService paymentService)
        {
            _trackService = trackService;
            _enrollmentService = enrollmentService;
            _paymentService = paymentService;
        }
        public async Task Handle(TransferEnrollmentCommand request, CancellationToken cancellationToken)
        {
            var trackCheck = await _trackService.CheckCapacityAsync(request.NewTrackId);
            if (!trackCheck) throw new Exception("No capacity");

            var updateEnroll =await _enrollmentService.UpdateTrackAsync(request.EnrollmentId, request.NewTrackId);
            if (!updateEnroll) throw new Exception("Update Track Failed");

            var payment = await _paymentService.GetByEnrollmentAsync(request.EnrollmentId);
            if(payment == null) throw new Exception("unable to get the payment");

            var track = await _trackService.GetByIdAsync(request.NewTrackId);

            var paymentResult = await _paymentService.UpdatePaymentAmountAsync(request.EnrollmentId,payment.Amount + (track.Fees) );

            return;
        }
    }
}
