using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Enrollments.Commands;
using LMS___Mini_Version.Services.Interfaces;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Handlers
{
    public class CancelEnrollmentCommandHandler : IRequestHandler<CancelEnrollmentCommand>
    {
        private readonly IEnrollmentService _enrollmentService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;
        public CancelEnrollmentCommandHandler(IEnrollmentService enrollmentService,IPaymentService paymentService,IUnitOfWork unitOfWork)
        {
            _enrollmentService = enrollmentService;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }
        public async Task Handle(CancelEnrollmentCommand request, CancellationToken cancellationToken)
        {
            var enrollment = await _enrollmentService.GetByIdAsync(request.EnrollmentId);
            if (enrollment == null) throw new Exception($"Not Found EnrollmentId {request.EnrollmentId}"); ;

            var statusResult = await _enrollmentService.UpdateStatusAsync(request.EnrollmentId, EnrollmentStatus.Cancelled);

            var paymentResult = await _paymentService.RefundPaymentAsync(request.EnrollmentId);
            if(statusResult && paymentResult)
                await _unitOfWork.CompleteAsync();
                
            return;
        }
    }
}
