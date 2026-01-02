
-- Drop all views
DO $$
DECLARE
r RECORD;
BEGIN
FOR r IN (SELECT table_schema, table_name 
              FROM information_schema.views 
              WHERE table_schema NOT IN ('pg_catalog', 'information_schema')
              ORDER BY table_name)
    LOOP
        EXECUTE 'DROP VIEW IF EXISTS ' || quote_ident(r.table_schema) || '.' || quote_ident(r.table_name) || ' CASCADE';
        RAISE NOTICE 'Dropped View: %.%', r.table_schema, r.table_name;
END LOOP;
END $$;

-- Drop all tables (this will also drop foreign keys)
DO $$
DECLARE
r RECORD;
BEGIN
FOR r IN (SELECT table_schema, table_name 
              FROM information_schema.tables 
              WHERE table_schema NOT IN ('pg_catalog', 'information_schema')
                AND table_type = 'BASE TABLE'
              ORDER BY table_name)
    LOOP
        EXECUTE 'DROP TABLE IF EXISTS ' || quote_ident(r.table_schema) || '.' || quote_ident(r.table_name) || ' CASCADE';
        RAISE NOTICE 'Dropped Table: %.%', r.table_schema, r.table_name;
END LOOP;
END $$;

-- Drop all functions
DO $$
DECLARE
r RECORD;
BEGIN
FOR r IN (SELECT n.nspname as schema_name, p.proname as function_name, pg_get_function_identity_arguments(p.oid) as args
              FROM pg_proc p
              JOIN pg_namespace n ON p.pronamespace = n.oid
              WHERE n.nspname NOT IN ('pg_catalog', 'information_schema')
                AND p.prokind = 'f'
              ORDER BY function_name)
    LOOP
        EXECUTE 'DROP FUNCTION IF EXISTS ' || quote_ident(r.schema_name) || '.' || quote_ident(r.function_name) || '(' || r.args || ') CASCADE';
        RAISE NOTICE 'Dropped Function: %.%', r.schema_name, r.function_name;
END LOOP;
END $$;

-- Drop all procedures
DO $$
DECLARE
r RECORD;
BEGIN
FOR r IN (SELECT n.nspname as schema_name, p.proname as procedure_name, pg_get_function_identity_arguments(p.oid) as args
              FROM pg_proc p
              JOIN pg_namespace n ON p.pronamespace = n.oid
              WHERE n.nspname NOT IN ('pg_catalog', 'information_schema')
                AND p.prokind = 'p'
              ORDER BY procedure_name)
    LOOP
        EXECUTE 'DROP PROCEDURE IF EXISTS ' || quote_ident(r.schema_name) || '.' || quote_ident(r.procedure_name) || '(' || r.args || ') CASCADE';
        RAISE NOTICE 'Dropped Procedure: %.%', r.schema_name, r.procedure_name;
END LOOP;
END $$;

-- Drop all sequences
DO $$
DECLARE
r RECORD;
BEGIN
FOR r IN (SELECT sequence_schema, sequence_name 
              FROM information_schema.sequences 
              WHERE sequence_schema NOT IN ('pg_catalog', 'information_schema')
              ORDER BY sequence_name)
    LOOP
        EXECUTE 'DROP SEQUENCE IF EXISTS ' || quote_ident(r.sequence_schema) || '.' || quote_ident(r.sequence_name) || ' CASCADE';
        RAISE NOTICE 'Dropped Sequence: %.%', r.sequence_schema, r.sequence_name;
END LOOP;
END $$;

-- Drop all custom types
DO $$
DECLARE
r RECORD;
BEGIN
FOR r IN (SELECT n.nspname as schema_name, t.typname as type_name
              FROM pg_type t
              JOIN pg_namespace n ON t.typnamespace = n.oid
              WHERE n.nspname NOT IN ('pg_catalog', 'information_schema')
                AND t.typtype = 'c'
              ORDER BY type_name)
    LOOP
        EXECUTE 'DROP TYPE IF EXISTS ' || quote_ident(r.schema_name) || '.' || quote_ident(r.type_name) || ' CASCADE';
        RAISE NOTICE 'Dropped Type: %.%', r.schema_name, r.type_name;
END LOOP;
END $$;