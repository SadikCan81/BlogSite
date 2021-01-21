# BlogSite
Basic Blog Site with Admin Panel (including Repository Pattern, UnitOfwork, Dapper and Identity) in .net core 3.1
For using dapper you should add your database these stored procedures:
------------------------------------------------------------------------
CREATE PROCEDURE usp_GetAllBlog
AS
Begin
SELECT blg.Id,blg.Title,blg.SubTitle,blg.Content,blg.ImagePath,blg.IsPublished,blg.CategoryId,blg.ApplicationUserId,blg.CreatedDate,ctg.Name,usr.FirstName + ' ' + usr.LastName as FullName
FROM dbo.Blogs blg 
INNER JOIN dbo.Categories ctg on blg.CategoryId = ctg.Id
INNER JOIN dbo.AspNetUsers usr on blg.ApplicationUserId = usr.Id
End
------------------------------------------------------------------------
CREATE PROCEDURE usp_GetAllCategory
AS
Begin
SELECT * FROM dbo.Categories
End
------------------------------------------------------------------------
