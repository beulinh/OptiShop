using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OptiShop.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$b6DGqudDCoDhJ8GNeuqZGe8z.rYc8npHGaknmlDp4TGhvppHOAd/W");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$LTpp6oW1TCpddrIW6cFAAeFwzVZ1FNbRmV2Z7VFNScOzj66J9GUFi");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                column: "PasswordHash",
                value: "$2a$11$BX9Q1xY8DRb3FNTIl1NffuEIEZDkwtQwol/abenxayIhS/Rb/0R3y");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$hwokNzVJZ/dpDYho405tJOVupLRiSYvT7NuEiCHjlMLW0jeiagvc2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$jeRh/y9ebyCWdtu50Qm8Ju5lObb75rLLyLwKhWQCQyJz4t4HrIN3u");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                column: "PasswordHash",
                value: "$2a$11$0kIg6JvIvVR4ws/cRBDAxuO9UukWLvYbNbigb5fKAp/ex.kuwGDf6");
        }
    }
}
