using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseWork.Migrations
{
    /// <inheritdoc />
    public partial class CommentUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentCommentId",
                table: "BlogComments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogComments_ParentCommentId",
                table: "BlogComments",
                column: "ParentCommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogComments_BlogComments_ParentCommentId",
                table: "BlogComments",
                column: "ParentCommentId",
                principalTable: "BlogComments",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogComments_BlogComments_ParentCommentId",
                table: "BlogComments");

            migrationBuilder.DropIndex(
                name: "IX_BlogComments_ParentCommentId",
                table: "BlogComments");

            migrationBuilder.DropColumn(
                name: "ParentCommentId",
                table: "BlogComments");
        }
    }
}
