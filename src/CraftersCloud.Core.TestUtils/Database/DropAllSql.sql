/* Azure friendly */
/* Drop all Foreign Key constraints */
DECLARE
@name VARCHAR(128)
DECLARE
@constraint VARCHAR(254)
DECLARE
@SQL VARCHAR(254)

SELECT @name = (SELECT TOP 1 TABLE_NAME
                FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
                WHERE constraint_catalog = DB_NAME()
                  AND CONSTRAINT_TYPE = 'FOREIGN KEY'
                ORDER BY TABLE_NAME)
           WHILE @name is not null
BEGIN
SELECT @constraint = (SELECT TOP 1 CONSTRAINT_NAME
                      FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
                      WHERE constraint_catalog = DB_NAME()
                        AND CONSTRAINT_TYPE = 'FOREIGN KEY'
                        AND TABLE_NAME = @name
                      ORDER BY CONSTRAINT_NAME)
           WHILE @constraint IS NOT NULL
BEGIN
SELECT @SQL = 'ALTER TABLE [dbo].[' + RTRIM(@name) + '] DROP CONSTRAINT [' + RTRIM(@constraint) + ']' EXEC (@SQL)
        PRINT 'Dropped FK Constraint: ' + @constraint + ' on ' + @name
SELECT @constraint = (SELECT TOP 1 CONSTRAINT_NAME
                      FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
                      WHERE constraint_catalog = DB_NAME()
                        AND CONSTRAINT_TYPE = 'FOREIGN KEY'
                        AND CONSTRAINT_NAME <> @constraint
                        AND TABLE_NAME = @name
                      ORDER BY CONSTRAINT_NAME)
END
SELECT @name = (SELECT TOP 1 TABLE_NAME
                FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
                WHERE constraint_catalog = DB_NAME()
                  AND CONSTRAINT_TYPE = 'FOREIGN KEY'
                ORDER BY TABLE_NAME)
END
GO

/* Drop all Primary Key constraints */
DECLARE
@name VARCHAR(128)
DECLARE
@constraint VARCHAR(254)
DECLARE
@SQL VARCHAR(254)

SELECT @name = (SELECT TOP 1 TABLE_NAME
                FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
                WHERE constraint_catalog = DB_NAME()
                  AND CONSTRAINT_TYPE = 'PRIMARY KEY'
                ORDER BY TABLE_NAME)
           WHILE @name IS NOT NULL
BEGIN
SELECT @constraint = (SELECT TOP 1 CONSTRAINT_NAME
                      FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
                      WHERE constraint_catalog = DB_NAME()
                        AND CONSTRAINT_TYPE = 'PRIMARY KEY'
                        AND TABLE_NAME = @name
                      ORDER BY CONSTRAINT_NAME)
           WHILE @constraint is not null
BEGIN
SELECT @SQL = 'ALTER TABLE [dbo].[' + RTRIM(@name) + '] DROP CONSTRAINT [' + RTRIM(@constraint) + ']' EXEC (@SQL)
        PRINT 'Dropped PK Constraint: ' + @constraint + ' on ' + @name
SELECT @constraint = (SELECT TOP 1 CONSTRAINT_NAME
                      FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
                      WHERE constraint_catalog = DB_NAME()
                        AND CONSTRAINT_TYPE = 'PRIMARY KEY'
                        AND CONSTRAINT_NAME <> @constraint
                        AND TABLE_NAME = @name
                      ORDER BY CONSTRAINT_NAME)
END
SELECT @name = (SELECT TOP 1 TABLE_NAME
                FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
                WHERE constraint_catalog = DB_NAME()
                  AND CONSTRAINT_TYPE = 'PRIMARY KEY'
                ORDER BY TABLE_NAME)
END
GO

/* Drop all tables */
DECLARE
@name VARCHAR(128)
DECLARE
@SQL VARCHAR(254)

SELECT @name = (SELECT TOP 1 [name]
FROM sysobjects
WHERE [type] = 'U' AND category = 0
ORDER BY [name])
    WHILE @name IS NOT NULL
BEGIN
SELECT @SQL = 'DROP TABLE [dbo].[' + RTRIM(@name) + ']' EXEC (@SQL)
    PRINT 'Dropped Table: ' + @name
SELECT @name = (SELECT TOP 1 [name]
FROM sysobjects
WHERE [type] = 'U' AND category = 0 AND [name] > @name
ORDER BY [name])
END
GO

/* Drop all views */
DECLARE
@name VARCHAR(128)
DECLARE
@SQL VARCHAR(254)

SELECT @name = (SELECT TOP 1 [name]
FROM sysobjects
WHERE [type] = 'V' AND category = 0
ORDER BY [name])
    WHILE @name IS NOT NULL
BEGIN
SELECT @SQL = 'DROP VIEW [dbo].[' + RTRIM(@name) + ']' EXEC (@SQL)
    PRINT 'Dropped View: ' + @name
SELECT @name = (SELECT TOP 1 [name]
FROM sysobjects
WHERE [type] = 'V' AND category = 0 AND [name] > @name
ORDER BY [name])
END
GO

/* Drop all functions */
Declare
@sql NVARCHAR(MAX) = N'';

SELECT @sql = @sql + N' DROP FUNCTION '
    + QUOTENAME(SCHEMA_NAME(schema_id))
    + N'.' + QUOTENAME(name)
FROM sys.objects
WHERE type_desc LIKE '%FUNCTION%';

Exec sp_executesql @sql

/* Drop all stored procedures */

declare
@procName varchar(500)
declare
cur cursor 

for
select [name]
from sys.objects
where type = 'p'
    open cur
    fetch next
from cur
into @procName
    while @@fetch_status = 0
begin
exec('drop procedure [' + @procName + ']')
    fetch next from cur into @procName
end
close cur deallocate cur