using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseWork.Migrations
{
    /// <inheritdoc />
    public partial class VoteCommentAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Votes_Blogs_BlogId",
                table: "Votes");

            migrationBuilder.AlterColumn<int>(
                name: "BlogId",
                table: "Votes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CommentsId",
                table: "Votes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Votes_CommentsId",
                table: "Votes",
                column: "CommentsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_BlogComments_CommentsId",
                table: "Votes",
                column: "CommentsId",
                principalTable: "BlogComments",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_Blogs_BlogId",
                table: "Votes",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Votes_BlogComments_CommentsId",
                table: "Votes");

            migrationBuilder.DropForeignKey(
                name: "FK_Votes_Blogs_BlogId",
                table: "Votes");

            migrationBuilder.DropIndex(
                name: "IX_Votes_CommentsId",
                table: "Votes");

            migrationBuilder.DropColumn(
                name: "CommentsId",
                table: "Votes");

            migrationBuilder.AlterColumn<int>(
                name: "BlogId",
                table: "Votes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_Blogs_BlogId",
                table: "Votes",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
