CREATE PROCEDURE GetAllProductsWithDetails
AS
BEGIN

    SELECT 
        p.Id,
        p.Name,
        p.Price,
        p.Category,
        p.Description,
        --alter
        -- Images
        pi.Id AS ImageId,
        pi.ProductId,
        pi.ImageUrl,
        pi.PublicId,
        pi.CreatedAt,

        -- Variants
        pv.Id AS VariantId,
        pv.Weight,
        pv.OriginalPrice,
        pv.SellPrice

    FROM Products p
    full outer join ProductImages pi ON pi.ProductId = p.Id
    full outer join ProductVariants pv ON pv.ProductId = p.Id
    WHERE p.IsActive = 1

END