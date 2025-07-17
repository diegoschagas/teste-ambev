using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales
{
    public class CreateSaleHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateSaleHandler> _logger;
        private readonly CreateSaleHandler _handler;
        private readonly IMediator _mediator;

        public CreateSaleHandlerTests(IMediator mediator)
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _mapper = Substitute.For<IMapper>();
            _logger = Substitute.For<ILogger<CreateSaleHandler>>();
            _mediator = mediator;

            _handler = new CreateSaleHandler(_saleRepository, _mapper, _logger, _mediator);
            
        }

        [Fact]
        public async Task Handle_QuantityBelow4_NoDiscount_ShouldCreateSale()
        {
            // Arrange
            var command = new CreateSaleCommand
            {
                SaleNumber = "S003",
                Date = DateTime.Today,
                Customer = "Customer C",
                Branch = "Branch Z",
                Items = new List<CreateSaleItemDto>
                {
                    new CreateSaleItemDto
                    {
                        Product = "P003",
                        Quantity = 3, // below 4
                        UnitPrice = 10,
                        Discount = 0 // no discount allowed
                    }
                }
            };

            var saleEntity = new Sale
            {
                SaleNumber = command.SaleNumber,
                Date = command.Date,
                Customer = command.Customer,
                Branch = command.Branch,
                Items = new List<SaleItem>
                {
                    new SaleItem
                    {
                        Product = "P003",
                        Quantity = 3,
                        UnitPrice = 10,
                        Discount = 0,
                        Total = 30
                    }
                },
                TotalAmount = 30
            };

            var expectedResult = new CreateSaleResult { SaleNumber = "S003" };

            _mapper.Map<Sale>(command).Returns(saleEntity);
            _mapper.Map<CreateSaleResult>(saleEntity).Returns(expectedResult);
            _saleRepository.AddAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
                .Returns(saleEntity);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.SaleNumber.Should().Be("S003");
        }

        [Fact]
        public async Task Handle_Quantity4_ShouldApply10PercentDiscount()
        {
            // Arrange
            var command = new CreateSaleCommand
            {
                SaleNumber = "S004",
                Date = DateTime.Today,
                Customer = "Customer D",
                Branch = "Branch W",
                Items = new List<CreateSaleItemDto>
                {
                    new CreateSaleItemDto
                    {
                        Product = "P004",
                        Quantity = 4, // triggers 10% discount
                        UnitPrice = 10,
                        Discount = 4 // expected discount value
                    }
                }
            };

            var saleEntity = new Sale
            {
                SaleNumber = command.SaleNumber,
                Date = command.Date,
                Customer = command.Customer,
                Branch = command.Branch,
                Items = new List<SaleItem>
                {
                    new SaleItem
                    {
                        Product = "P004",
                        Quantity = 4,
                        UnitPrice = 10,
                        Discount = 10,
                        Total = 36
                    }
                },
                TotalAmount = 36
            };

            var expectedResult = new CreateSaleResult { SaleNumber = "S004" };

            _mapper.Map<Sale>(command).Returns(saleEntity);
            _mapper.Map<CreateSaleResult>(saleEntity).Returns(expectedResult);
            _saleRepository.AddAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
                .Returns(saleEntity);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.SaleNumber.Should().Be("S004");
        }

        [Fact]
        public async Task Handle_Quantity10_ShouldApply20PercentDiscount()
        {
            // Arrange
            var command = new CreateSaleCommand
            {
                SaleNumber = "S005",
                Date = DateTime.Today,
                Customer = "Customer E",
                Branch = "Branch V",
                Items = new List<CreateSaleItemDto>
                {
                    new CreateSaleItemDto
                    {
                        Product = "P005",
                        Quantity = 10, // triggers 20% discount
                        UnitPrice = 10,
                        Discount = 20 // expected discount value
                    }
                }
            };

            var saleEntity = new Sale
            {
                SaleNumber = command.SaleNumber,
                Date = command.Date,
                Customer = command.Customer,
                Branch = command.Branch,
                Items = new List<SaleItem>
                {
                    new SaleItem
                    {
                        Product = "P005",
                        Quantity = 10,
                        UnitPrice = 10,
                        Discount = 20,
                        Total = 80
                    }
                },
                TotalAmount = 80
            };

            var expectedResult = new CreateSaleResult { SaleNumber = "S005" };

            _mapper.Map<Sale>(command).Returns(saleEntity);
            _mapper.Map<CreateSaleResult>(saleEntity).Returns(expectedResult);
            _saleRepository.AddAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
                .Returns(saleEntity);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.SaleNumber.Should().Be("S005");
        }

        [Fact]
        public async Task Handle_QuantityAbove20_ShouldThrowValidationException()
        {
            // Arrange
            var command = new CreateSaleCommand
            {
                SaleNumber = "S006",
                Date = DateTime.Today,
                Customer = "Customer F",
                Branch = "Branch U",
                Items = new List<CreateSaleItemDto>
        {
            new CreateSaleItemDto
            {
                Product = "P006",
                Quantity = 25, // exceeds max allowed
                UnitPrice = 10,
                Discount = 0
            }
        }
            };

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("*Item 'P006' cannot have quantity above 20*");
        }

        [Fact]
        public async Task Handle_QuantityBelow4_WithDiscount_ShouldThrowValidationException()
        {
            // Arrange
            var command = new CreateSaleCommand
            {
                SaleNumber = "S007",
                Date = DateTime.Today,
                Customer = "Customer G",
                Branch = "Branch T",
                Items = new List<CreateSaleItemDto>
        {
            new CreateSaleItemDto
            {
                Product = "P007",
                Quantity = 2,
                UnitPrice = 10,
                Discount = 5
            }
        }
            };

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("*Item 'P007' cannot have a discount for quantities below 4*");
        }
    }
}
