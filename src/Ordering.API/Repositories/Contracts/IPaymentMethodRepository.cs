using Ordering.API.BuyerModel;
using Ordering.API.Model;

namespace Ordering.API.Repositories.Contracts
{
    public interface IPaymentMethodRepository
    {
        public Task<PaymentMethod> AddPaymentMethod(PaymentMethod paymentMethod);
        public Task<PaymentMethod> UpdatePaymentMethod(PaymentMethod paymentMethod);
        public Task<PaymentMethod> GetPaymentMethodByIdAsync(int paymentMethodId);
        public Task<PaginatedItems<PaymentMethod>> GetPaymentMethodFromUserAsync(Guid buyerId, int pageIndex, int pageSize);
        public Task DeletePaymentMethod(PaymentMethod paymentMethod);
        public Task SaveChangesAsync();
    }
}
