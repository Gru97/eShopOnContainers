using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Ordering.Application.Commands;

namespace Ordering.Application.Validation
{
    //To validate commands that we're getting from front-end
    //https://fluentvalidation.net/aspnet
    //http://softdevben.blogspot.com/2017/12/using-mediatr-pipeline-with-fluent.html
    public class CreateOrderCommandValidator:AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(command => command.ZipCode).NotEmpty().WithMessage("ورود کد پستی اجباری است");
            RuleFor(command => command.City).NotEmpty().WithMessage("ورود نام شهر اجباری است");
            RuleFor(command => command.Country).NotEmpty().WithMessage("ورود نام کشور اجباری است");
            RuleFor(command => command.State).NotEmpty().WithMessage("ورود نام استان اجباری است");
            RuleFor(command => command.Street).NotEmpty().WithMessage("ورود نام خیابان اجباری است");
            RuleFor(command => command.OrderItems).Must(ContainItems).WithMessage("لیست آیتم های سفارش نمیتواند خالی باشد");
        }

        private bool ContainItems(IEnumerable<OrderItemDto> items)
        {
            return items.Any();
        }
    }
}
