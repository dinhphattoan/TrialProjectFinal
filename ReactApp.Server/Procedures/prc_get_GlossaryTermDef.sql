CREATE PROCEDURE GetCustomerOrder
    @startIndex INT,
    @length INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT *
    FROM (
        SELECT *, ROW_NUMBER() OVER (ORDER BY TermOfPhrase) AS RN
        FROM Glossaries
    ) AS Subquery
    WHERE RN BETWEEN @startIndex AND @startIndex + @length - 1;
END;
