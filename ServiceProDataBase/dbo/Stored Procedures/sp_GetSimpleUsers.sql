CREATE  PROCEDURE dbo.sp_GetSimpleUsers
AS
BEGIN
    SELECT *
    FROM products p
    LEFT JOIN productvariants pv with (nolock)
        ON pv.productid = p.id AND pv.isactive = 1
    LEFT JOIN productimages pi with (nolock)
        ON pi.productid = p.id
    WHERE p.isactive = 1

END
select * from products
delete from products where name ='4765'