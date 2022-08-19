using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SamuraiApp.Data.Migrations
{
    public partial class newsprocs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"create procedure dbo.SamuraisWhoSaidAWord
                  @text varchar(20)
                  as
                  select Samurais.Id, Samurais.Name
                  from Samurais inner join
                        Quotes on Samurais.Id = Quotes.SamuraiId
                  where (Quotes.Text like '%'+@text+'%')");
            migrationBuilder.Sql(
                @"create procedure dbo.DeleteQuotesFromSamurai
                  @samuraiId int
                  as
                  delete from Quotes
                  where Quotes.SamuraiId=@samuraiId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"drop procedure dbo.SamuraiWhoSaidAWord");
            migrationBuilder.Sql(@"drop procedure dbo.DeleteQuotesFromSamurai");
        }
        
    }
}
