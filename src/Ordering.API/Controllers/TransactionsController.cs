using BookCatalog.API.Queries.Mappers;
using Microsoft.AspNetCore.Mvc;
using Ordering.API.Model;
using Ordering.API.Models.DTOs;
using Ordering.API.Models.OrderModel;
using Ordering.API.Repositories;

namespace Ordering.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]

    public class TransactionsController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly ITransactionRepository _transactionRepository;

        public TransactionsController(ILogger<OrdersController> logger, ITransactionRepository transactionRepository)
        {
            _logger = logger;
            _transactionRepository = transactionRepository;
        }

        [HttpGet("{transactionId}")]
        public async Task<IActionResult> GetTransactionByIdAsync([FromRoute] int transactionId)
        {
            var transaction = await _transactionRepository.GetTransactionByIdAsync(transactionId);

            if (transaction == null)
            {
                return NotFound("Transaction not found");
            }

            return Ok(OrderMapper.ToTransactionDetailDTO(transaction));
        }
        [HttpGet("buyer/{buyerId}")]
        public async Task<IActionResult> GetOrdersFromBuyerAsync([FromRoute] Guid buyerId, [FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10)
        {
            var transactions = await _transactionRepository.GetTransactionsFromUserAsync(buyerId, pageIndex, pageSize);

            if (transactions == null || transactions.TotalItems == 0)
            {
                return NotFound("Transactions not found");
            }

            return Ok(new PaginatedItems<TransactionDetailDTO>(
                transactions.PageIndex,
                transactions.PageSize,
                transactions.TotalItems,
                transactions.Data.Select(
                    transaction => OrderMapper.ToTransactionDetailDTO(transaction))
                .ToList()));
        }

        [HttpGet("")]
        public async Task<IActionResult> GetTransactions([FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10)
        {
            var transactions = await _transactionRepository.GetTransactions(pageIndex, pageSize);

            if (transactions == null || transactions.TotalItems == 0)
            {
                return NotFound("Transactions not found");
            }

            return Ok(new PaginatedItems<TransactionDetailDTO>(
                transactions.PageIndex,
                transactions.PageSize,
                transactions.TotalItems,
                transactions.Data.Select(
                    transaction => OrderMapper.ToTransactionDetailDTO(transaction))
                .ToList()));
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateTransactionAsync([FromBody] CreateTransactionDTO createTransaction)
        {
            var transaction = OrderMapper.ToTransaction(createTransaction);
            _transactionRepository.AddTransaction(transaction);
            await _transactionRepository.SaveChangesAsync();

            return Ok(transaction);
        }

        [HttpPatch("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateTransactionAsync([FromBody] TransactionDetailDTO transaction)
        {
            var existingTransaction = await _transactionRepository.GetTransactionByIdAsync(transaction.Id);

            if (existingTransaction == null)
            {
                return NotFound("Transaction not found for update");
            }

            _transactionRepository.UpdateTransaction(OrderMapper.ToTransactionFromDTO(transaction));
            await _transactionRepository.SaveChangesAsync();

            return Ok("Format updated successfully");
        }

        [HttpDelete("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteTransactionAsync([FromQuery] int id)
        {
            var existingFormat = await _transactionRepository.GetTransactionByIdAsync(id);

            if (existingFormat == null)
            {
                return NotFound("Transaction not found for delete");
            }

            _transactionRepository.DeleteTransaction(new Transaction { Id = id });
            await _transactionRepository.SaveChangesAsync();

            return Ok("Format deleted successfully");
        }
    }
}
