﻿namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
    public record StoreBasketResult(string UserName);

    public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketCommandValidator()
        {
            RuleFor(x => x.Cart).NotNull().WithMessage("Cart cannot be null.");
            RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("UserName os required");
        }
    }

    public class StoreBasketCommandHandler(IBasketRepository repository) 
        : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            ShoppingCart cart = command.Cart;
            //TODO: store basket in database (use Marten upsert - if exist = update, if not = insert)
            //TODO: update cache
            await repository.StoreBasket(cart, cancellationToken);

            return new StoreBasketResult(cart.UserName);
        }
    }
}
