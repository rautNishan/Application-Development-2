using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseWork.Migrations
{
    /// <inheritdoc />
    public partial class CommentUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogComments_Blogs_BlogEntityId",
                table: "BlogComments");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "BlogComments",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "BlogComments",
                newName: "CommentedUserId");

            migrationBuilder.RenameColumn(
                name: "BlogEntityId",
                table: "BlogComments",
                newName: "BlogId");

            migrationBuilder.RenameIndex(
                name: "IX_BlogComments_BlogEntityId",
                table: "BlogComments",
                newName: "IX_BlogComments_BlogId");

            migrationBuilder.AddColumn<string>(
                name: "CommentedUserName",
                table: "BlogComments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "BlogComments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "BlogComments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "BlogComments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogComments_Blogs_BlogId",
                table: "BlogComments",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogComments_Blogs_BlogId",
                table: "BlogComments");

            migrationBuilder.DropColumn(
                name: "CommentedUserName",
                table: "BlogComments");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "BlogComments");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "BlogComments");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "BlogComments");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "BlogComments",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "CommentedUserId",
                table: "BlogComments",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "BlogId",
                table: "BlogComments",
                newName: "BlogEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_BlogComments_BlogId",
                table: "BlogComments",
                newName: "IX_BlogComments_BlogEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogComments_Blogs_BlogEntityId",
                table: "BlogComments",
                column: "BlogEntityId",
                principalTable: "Blogs",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
