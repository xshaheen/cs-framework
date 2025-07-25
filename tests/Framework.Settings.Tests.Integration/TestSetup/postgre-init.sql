﻿CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
                                                       "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
    );

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250118002326_InitialMigration') THEN
        IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'settings') THEN
CREATE SCHEMA settings;
END IF;
END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250118002326_InitialMigration') THEN
CREATE TABLE settings."SettingDefinitions" (
                                               "Id" uuid NOT NULL,
                                               "Name" character varying(128) NOT NULL,
                                               "DisplayName" character varying(256) NOT NULL,
                                               "Description" character varying(512),
                                               "DefaultValue" character varying(2000),
                                               "IsVisibleToClients" boolean NOT NULL,
                                               "IsInherited" boolean NOT NULL,
                                               "IsEncrypted" boolean NOT NULL,
                                               "Providers" character varying(1024),
                                               "ExtraProperties" text NOT NULL,
                                               CONSTRAINT "PK_SettingDefinitions" PRIMARY KEY ("Id")
);
END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250118002326_InitialMigration') THEN
CREATE TABLE settings."SettingValues" (
                                          "Id" uuid NOT NULL,
                                          "Name" character varying(128) NOT NULL,
                                          "Value" character varying(2000) NOT NULL,
                                          "ProviderName" character varying(64) NOT NULL,
                                          "ProviderKey" character varying(64),
                                          CONSTRAINT "PK_SettingValues" PRIMARY KEY ("Id")
);
END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250118002326_InitialMigration') THEN
CREATE UNIQUE INDEX "IX_SettingDefinitions_Name" ON settings."SettingDefinitions" ("Name");
END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250118002326_InitialMigration') THEN
CREATE UNIQUE INDEX "IX_SettingValues_Name_ProviderName_ProviderKey" ON settings."SettingValues" ("Name", "ProviderName", "ProviderKey");
END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250118002326_InitialMigration') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20250118002326_InitialMigration', '9.0.1');
END IF;
END $EF$;
COMMIT;