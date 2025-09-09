using System.Collections.Generic;
using System.Threading.Tasks;
using CarManagementApp.Controllers;
using CarManagementApp.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Testy.Controllers;

public class PojazdyControllerTests
{
    [Fact]
    public async Task Index_ReturnsView_WithListOfVehicles()
    {
        // arrange
        await using var ctx = Testy.Infrastructure.TestDbContextFactory.Create();
        ctx.Pojazdy.AddRange(
            new Pojazd { NumerRejestracyjny = "WX11111", VIN = "VIN111", Status = "Aktywny" },
            new Pojazd { NumerRejestracyjny = "WX22222", VIN = "VIN222", Status = "Aktywny" }
        );
        await ctx.SaveChangesAsync();

        var controller = new PojazdyController(ctx, new NullLogger<PojazdyController>());

        // act
        var result = await controller.Index();

        // assert
        result.Should().BeOfType<ViewResult>();
        var view = (ViewResult)result;
        view.Model.Should().NotBeNull().And.BeAssignableTo<IEnumerable<Pojazd>>();
        var model = (IEnumerable<Pojazd>)view.Model!;
        model.Should().HaveCount(2);
    }

    [Fact]
    public async Task Index_ReturnsEmptyList_WhenNoVehicles()
    {
        // arrange
        await using var ctx = Testy.Infrastructure.TestDbContextFactory.Create();
        var controller = new PojazdyController(ctx, new NullLogger<PojazdyController>());

        // act
        var result = await controller.Index();

        // assert
        var view = result as ViewResult;
        view.Should().NotBeNull();
        var model = view!.Model as IEnumerable<Pojazd>;
        model.Should().NotBeNull().And.BeEmpty();
    }
}