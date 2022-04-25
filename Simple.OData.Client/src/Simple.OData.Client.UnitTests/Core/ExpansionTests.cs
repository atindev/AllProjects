﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Simple.OData.Client.Tests.Core
{
    public class ExpansionTests : CoreTestBase
    {
        public override string MetadataFile { get { return "Northwind3.xml"; } }
        public override IFormatSettings FormatSettings { get { return new ODataV3Format(); } }

        [Theory]
        [InlineData("Northwind3.xml", "Employees?$expand=Subordinates")]
        [InlineData("Northwind4.xml", "Employees?$expand=Subordinates")]
        public async Task ExpandSubordinates(string metadataFile, string expectedCommand)
        {
            var client = CreateClient(metadataFile);
            var command = client
                .For<Employee>()
                .Expand(x => x.Subordinates);
            var commandText = await command.GetCommandTextAsync();
            Assert.Equal(expectedCommand, commandText);
        }

        [Theory]
        [InlineData("Northwind3.xml", "Employees?$expand=Subordinates,Superior")]
        [InlineData("Northwind4.xml", "Employees?$expand=Subordinates,Superior")]
        public async Task ExpandSubordinatesAndSuperior(string metadataFile, string expectedCommand)
        {
            var client = CreateClient(metadataFile);
            var command = client
                .For<Employee>()
                .Expand(x => new { x.Subordinates, x.Superior });
            var commandText = await command.GetCommandTextAsync();
            Assert.Equal(expectedCommand, commandText);
        }

        [Theory]
        [InlineData("Northwind3.xml", "Employees?$expand=Subordinates,Superior")]
        [InlineData("Northwind4.xml", "Employees?$expand=Subordinates,Superior")]
        public async Task ExpandSubordinatesAndSuperiorTwoClauses(string metadataFile, string expectedCommand)
        {
            var client = CreateClient(metadataFile);
            var command = client
                .For<Employee>()
                .Expand(x => x.Subordinates)
                .Expand(x => x.Superior);
            var commandText = await command.GetCommandTextAsync();
            Assert.Equal(expectedCommand, commandText);
        }

        [Theory]
        [InlineData("Northwind3.xml", "Employees?$expand=Subordinates/Subordinates")]
        [InlineData("Northwind4.xml", "Employees?$expand=Subordinates($expand=Subordinates)")]
        public async Task ExpandSubordinatesTwoTimes(string metadataFile, string expectedCommand)
        {
            var client = CreateClient(metadataFile);
            var command = client
                .For<Employee>()
                .Expand(x => x.Subordinates.Select(y => y.Subordinates));
            var commandText = await command.GetCommandTextAsync();
            Assert.Equal(expectedCommand, commandText);
        }

        [Theory]
        [InlineData("Northwind3.xml", "Employees?$expand=Subordinates/Subordinates/Subordinates")]
        [InlineData("Northwind4.xml", "Employees?$expand=Subordinates($expand=Subordinates($expand=Subordinates))")]
        public async Task ExpandSubordinatesThreeTimes(string metadataFile, string expectedCommand)
        {
            var client = CreateClient(metadataFile);
            var command = client
                .For<Employee>()
                .Expand(x => x.Subordinates.Select(y => y.Subordinates.Select(z => z.Subordinates)));
            var commandText = await command.GetCommandTextAsync();
            Assert.Equal(expectedCommand, commandText);
        }

        [Theory]
        [InlineData("Northwind3.xml", "Employees?$expand=Subordinates&$select=LastName,Subordinates&$orderby=LastName")]
        [InlineData("Northwind4.xml", "Employees?$expand=Subordinates&$select=LastName,Subordinates&$orderby=LastName")]
        public async Task ExpandSubordinatesWithSelectAndOrderby(string metadataFile, string expectedCommand)
        {
            var client = CreateClient(metadataFile);
            var command = client
                .For<Employee>()
                .Expand(x => x.Subordinates)
                .Select(x => new { x.LastName, x.Subordinates })
                .OrderBy(x => x.LastName);
            var commandText = await command.GetCommandTextAsync();
            Assert.Equal(expectedCommand, commandText);
        }

        [Theory]
        [InlineData("Northwind3.xml", "Employees?$expand=Subordinates/Subordinates&$select=LastName,Subordinates,Subordinates/LastName,Subordinates/Subordinates")]
        [InlineData("Northwind4.xml", "Employees?$expand=Subordinates($expand=Subordinates;$select=LastName,Subordinates)&$select=LastName,Subordinates")]
        public async Task ExpandSubordinatesWithInnerSelect(string metadataFile, string expectedCommand)
        {
            var client = CreateClient(metadataFile);
            var command = client
                .For<Employee>()
                .Expand(x => x.Subordinates.Select(y => y.Subordinates))
                .Select(x => new { x.LastName, x.Subordinates })
                .Select(x => x.Subordinates.Select(y => new { y.LastName, y.Subordinates }));
            var commandText = await command.GetCommandTextAsync();
            Assert.Equal(expectedCommand, commandText);
        }

        [Theory]
        [InlineData("Northwind3.xml", "Employees?$expand=Subordinates/Subordinates&$select=LastName,Subordinates,Subordinates/LastName,Subordinates/Subordinates&$orderby=LastName,Subordinates/LastName")]
        [InlineData("Northwind4.xml", "Employees?$expand=Subordinates($expand=Subordinates;$select=LastName,Subordinates;$orderby=LastName)&$select=LastName,Subordinates&$orderby=LastName")]
        public async Task ExpandSubordinatesWithSelectAndOrderbyTwoTimes(string metadataFile, string expectedCommand)
        {
            var client = CreateClient(metadataFile);
            var command = client
                .For<Employee>()
                .Expand(x => x.Subordinates.Select(y => y.Subordinates))
                .Select(x => new { x.LastName, x.Subordinates })
                .Select(x => x.Subordinates.Select(y => new { y.LastName, y.Subordinates }))
                .OrderBy(x => x.LastName)
                .OrderBy(x => x.Subordinates.Select(y => y.LastName));
            var commandText = await command.GetCommandTextAsync();
            Assert.Equal(expectedCommand, commandText);
        }

        [Theory]
        [InlineData("Northwind3.xml", null)]
        [InlineData("Northwind4.xml", null)]
        public async Task ExpandSubordinatesWithSelectAndInnerOrderby(string metadataFile, string expectedCommand)
        {
            var client = CreateClient(metadataFile);
            Assert.Throws<NotSupportedException>(() => client
                .For<Employee>()
                .Expand(x => x.Subordinates.Select(y => y.Subordinates))
                .Select(x => x.Subordinates.Select(y => new { y.LastName, y.Subordinates }).OrderBy(y => y.LastName)));
        }

        [Theory]
        [InlineData("Northwind3.xml", "Employees?$expand=Subordinates/Subordinates/Subordinates&$select=LastName,Subordinates,Subordinates/LastName,Subordinates/Subordinates,Subordinates/Subordinates/LastName,Subordinates/Subordinates/Subordinates&$orderby=LastName,Subordinates/LastName,Subordinates/Subordinates/LastName")]
        [InlineData("Northwind4.xml", "Employees?$expand=Subordinates($expand=Subordinates($expand=Subordinates;$select=LastName,Subordinates;$orderby=LastName);$select=LastName,Subordinates;$orderby=LastName)&$select=LastName,Subordinates&$orderby=LastName")]
        public async Task ExpandSubordinatesWithSelectAndOrderbyThreeTimes(string metadataFile, string expectedCommand)
        {
            var client = CreateClient(metadataFile);
            var command = client
                .For<Employee>()
                .Expand(x => x.Subordinates.Select(y => y.Subordinates.Select(z => z.Subordinates)))
                .Select(x => new { x.LastName, x.Subordinates })
                .Select(x => x.Subordinates.Select(y => new { y.LastName, y.Subordinates }))
                .Select(x => x.Subordinates.Select(y => y.Subordinates.Select(z => new { z.LastName, z.Subordinates })))
                .OrderBy(x => x.LastName)
                .OrderBy(x => x.Subordinates.Select(y => y.LastName))
                .OrderBy(x => x.Subordinates.Select(y => y.Subordinates.Select(z => z.LastName)));
            var commandText = await command.GetCommandTextAsync();
            Assert.Equal(expectedCommand, commandText);
        }

        [Theory]
        [InlineData("Northwind3.xml", "Employees?$expand=Subordinates&$select=LastName,Subordinates&$orderby=Superior/LastName")]
        [InlineData("Northwind4.xml", "Employees?$expand=Subordinates&$select=LastName,Subordinates&$orderby=Superior/LastName")]
        public async Task ExpandSubordinatesWithSelectThenDeepOrderby(string metadataFile, string expectedCommand)
        {
            var client = CreateClient(metadataFile);
            var command = client
                .For<Employee>()
                .Expand(x => x.Subordinates)
                .Select(x => new { x.LastName, x.Subordinates })
                .OrderBy(x => x.Superior.LastName);
            var commandText = await command.GetCommandTextAsync();
            Assert.Equal(expectedCommand, commandText);
        }

        [Theory]
        [InlineData("Northwind3.xml", "Employees?$select=LastName,Subordinates&$orderby=Superior/LastName")]
        [InlineData("Northwind4.xml", "Employees?$select=LastName,Subordinates&$orderby=Superior/LastName")]
        public async Task SelectAndDeepOrderby(string metadataFile, string expectedCommand)
        {
            var client = CreateClient(metadataFile);
            var command = client
                .For<Employee>()
                .Select(x => new { x.LastName, x.Subordinates })
                .OrderBy(x => x.Superior.LastName);
            var commandText = await command.GetCommandTextAsync();
            Assert.Equal(expectedCommand, commandText);
        }

        [Theory]
        [InlineData("Northwind3.xml", "Products?$expand=Category&$select=ProductName,Category/CategoryName")]
        [InlineData("Northwind4.xml", "Products?$expand=Category($select=CategoryName)&$select=ProductName")]
        public async Task ExpandCategorySelectProductNameCategoryName(string metadataFile, string expectedCommand)
        {
            var client = CreateClient(metadataFile);
            var command = client.For<Product>()
                .Expand(p => p.Category)
                .Select(p => new { p.ProductName, p.Category.CategoryName });
            var commandText = await command.GetCommandTextAsync();
            Assert.Equal(expectedCommand, commandText);
        }

        [Theory]
        [InlineData("Northwind3.xml", "Products?$expand=Category&$select=ProductName,Category/CategoryName&$orderby=Category/CategoryName")]
        [InlineData("Northwind4.xml", "Products?$expand=Category($select=CategoryName)&$select=ProductName&$orderby=Category/CategoryName")]
        public async Task ExpandCategorySelectProductNameCategoryNameThenOrderBy(string metadataFile, string expectedCommand)
        {
            var client = CreateClient(metadataFile);
            var command = client.For<Product>()
                .Expand(p => p.Category)
                .Select(p => new {p.ProductName, p.Category.CategoryName})
                .OrderBy(p => p.Category.CategoryName);
            var commandText = await command.GetCommandTextAsync();
            Assert.Equal(expectedCommand, commandText);
        }

        [Theory]
        [InlineData("Northwind3.xml", "Products?$expand=Category&$select=ProductName,Category/CategoryName&$orderby=Category/CategoryName,ProductName")]
        [InlineData("Northwind4.xml", "Products?$expand=Category($select=CategoryName)&$select=ProductName&$orderby=Category/CategoryName,ProductName")]
        public async Task ExpandCategorySelectProductNameCategoryNameThenOrderByCategoryNameThenByProductName(string metadataFile, string expectedCommand)
        {
            var client = CreateClient(metadataFile);
            var command = client.For<Product>()
                .Expand(p => p.Category)
                .Select(p => new { p.ProductName, p.Category.CategoryName })
                .OrderBy(p => p.Category.CategoryName)
                .ThenBy(p => p.ProductName);
            var commandText = await command.GetCommandTextAsync();
            Assert.Equal(expectedCommand, commandText);
        }

        [Fact]
        public async Task ExpandSubordinates2LevelsByValue()
        {
            var client = CreateClient("Northwind4.xml");
            var command = client
                .For<Employee>()
                .Expand(ODataExpandOptions.ByValue(2), x => x.Subordinates);

            var commandText = await command.GetCommandTextAsync();
            Assert.Equal("Employees?$expand=Subordinates($levels=2)", commandText);
        }

        [Fact]
        public async Task ExpandSubordinates2LevelsByReference()
        {
            var client = CreateClient("Northwind4.xml");
            var command = client
                .For<Employee>()
                .Expand(ODataExpandOptions.ByReference(2), x => x.Subordinates);

            var commandText = await command.GetCommandTextAsync();
            Assert.Equal("Employees?$expand=Subordinates/$ref($levels=2)", commandText);
        }

        [Fact]
        public async Task ExpandFunction()
        {
            var x = ODataDynamic.Expression;
            var client = CreateClient("TripPin.xml");
            var command = client
                .For(x.Person)
                .Key("scottketchum")
                .NavigateTo(x.Trip)
                .Key(0)
                .Function("GetInvolvedPeople")
                .Expand(x.Photos);

            var commandText = await command.GetCommandTextAsync();
            Assert.Equal("People(%27scottketchum%27)/Trips(0)/Microsoft.OData.SampleService.Models.TripPin.GetInvolvedPeople()?$expand=Photo", commandText);
        }

        [Fact]
        public async Task ExpandFunctionMultipleLevelsWithSelect()
        {
            var client = CreateClient("ClientProductSku.xml");

            string[] CreateUpdateExpandTables = {
                "Product/ProductCategory/Category/CategorySalesArea",            
                "ClientProductSkuPriceList",
                "ClientProductSkuSalesArea",
                "Product/SupplierProductSkuClient/SupplierProductSku/SupplierProductSkuPriceList/SupplierPriceList",
                "Product/SupplierProductSkuClient/SupplierProductSku/SupplierProductSkuOnHand/Warehouse"
            };

            string[] CreateUpdateSelectColumns = {
                "PartNo", "ClientId", "ErpName", "EanCode", 
                "Product/Id","Product/ManufacturerId",
                "Product/ProductCategory/IsPrimary",
                "Product/ProductCategory/Category/Code", 
                "ClientProductSkuPriceList/CurrencyId"
            };

            var expectedResult =
                @"ClientProductSkus/FunctionService.GetCreateUpdateSkuDelta(clientId=35,offsetInMinutes=2000)?" +
                "$expand=Product($expand=ProductCategory($expand=Category($expand=CategorySalesArea;$select=Code);$select=IsPrimary);$select=Id,ManufacturerId)," +
                "ClientProductSkuPriceList($select=CurrencyId)," +
                "ClientProductSkuSalesArea," +
                "Product($expand=SupplierProductSkuClient($expand=SupplierProductSku($expand=SupplierProductSkuPriceList($expand=SupplierPriceList)));$select=Id,ManufacturerId)," +
                "Product($expand=SupplierProductSkuClient($expand=SupplierProductSku($expand=SupplierProductSkuOnHand($expand=Warehouse)));$select=Id,ManufacturerId)&" +
                "$select=PartNo,ClientId,ErpName,EanCode";

            var clientId = 35;
            var offsetInMinutes = 2000;

            var command = client.For("ClientProductSku")
                .Function("GetCreateUpdateSkuDelta")
                .Set(new { clientId, offsetInMinutes })
                    .Expand(CreateUpdateExpandTables)
                    .Select(CreateUpdateSelectColumns);

            var commandText = await command.GetCommandTextAsync();
            Console.WriteLine(commandText);

            Assert.Equal(expectedResult, commandText);
        }
    }
}