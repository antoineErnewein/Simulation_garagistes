
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 05/14/2014 15:32:13
-- Generated from EDMX file: C:\Users\Antoine\Documents\GitHub\Simulation_garagistes\DAL\Models\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [GESTIONGARAGE];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_GaragistePeriodeFermeture]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PeriodeFermetureJeu] DROP CONSTRAINT [FK_GaragistePeriodeFermeture];
GO
IF OBJECT_ID(N'[dbo].[FK_FranchiseGaragiste]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GaragisteJeu] DROP CONSTRAINT [FK_FranchiseGaragiste];
GO
IF OBJECT_ID(N'[dbo].[FK_GaragisteReparation]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ReparationJeu] DROP CONSTRAINT [FK_GaragisteReparation];
GO
IF OBJECT_ID(N'[dbo].[FK_VoitureReparation]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ReparationJeu] DROP CONSTRAINT [FK_VoitureReparation];
GO
IF OBJECT_ID(N'[dbo].[FK_ModeleVoiture]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[VoitureJeu] DROP CONSTRAINT [FK_ModeleVoiture];
GO
IF OBJECT_ID(N'[dbo].[FK_MarqueModele]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ModeleJeu] DROP CONSTRAINT [FK_MarqueModele];
GO
IF OBJECT_ID(N'[dbo].[FK_ModeleRevision]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RevisionJeu] DROP CONSTRAINT [FK_ModeleRevision];
GO
IF OBJECT_ID(N'[dbo].[FK_MarqueRevision]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RevisionJeu] DROP CONSTRAINT [FK_MarqueRevision];
GO
IF OBJECT_ID(N'[dbo].[FK_ReparationRevision]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ReparationJeu] DROP CONSTRAINT [FK_ReparationRevision];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[GaragisteJeu]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GaragisteJeu];
GO
IF OBJECT_ID(N'[dbo].[VoitureJeu]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VoitureJeu];
GO
IF OBJECT_ID(N'[dbo].[PeriodeFermetureJeu]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PeriodeFermetureJeu];
GO
IF OBJECT_ID(N'[dbo].[RevisionJeu]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RevisionJeu];
GO
IF OBJECT_ID(N'[dbo].[FranchiseJeu]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FranchiseJeu];
GO
IF OBJECT_ID(N'[dbo].[ReparationJeu]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReparationJeu];
GO
IF OBJECT_ID(N'[dbo].[ModeleJeu]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ModeleJeu];
GO
IF OBJECT_ID(N'[dbo].[MarqueJeu]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MarqueJeu];
GO
IF OBJECT_ID(N'[dbo].[LogJeu]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LogJeu];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'GaragisteJeu'
CREATE TABLE [dbo].[GaragisteJeu] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Nom] nvarchar(max)  NOT NULL,
    [Franchise_ID] int  NOT NULL
);
GO

-- Creating table 'VoitureJeu'
CREATE TABLE [dbo].[VoitureJeu] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Kilometrage] float  NULL,
    [Modele_ID] int  NOT NULL
);
GO

-- Creating table 'PeriodeFermetureJeu'
CREATE TABLE [dbo].[PeriodeFermetureJeu] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [DateDebut] datetime  NOT NULL,
    [DateFin] datetime  NOT NULL,
    [Garagiste_ID] int  NOT NULL
);
GO

-- Creating table 'RevisionJeu'
CREATE TABLE [dbo].[RevisionJeu] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Libelle] nvarchar(max)  NOT NULL,
    [Kilometrage] float  NOT NULL,
    [DureeIntervention] time  NOT NULL,
    [Modele_ID] int  NULL,
    [Marque_ID] int  NULL
);
GO

-- Creating table 'FranchiseJeu'
CREATE TABLE [dbo].[FranchiseJeu] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Nom] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'ReparationJeu'
CREATE TABLE [dbo].[ReparationJeu] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [DateDebut] datetime  NOT NULL,
    [DateFin] datetime  NOT NULL,
    [Garagiste_ID] int  NOT NULL,
    [Voiture_ID] int  NOT NULL,
    [Revision_ID] int  NOT NULL
);
GO

-- Creating table 'ModeleJeu'
CREATE TABLE [dbo].[ModeleJeu] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Nom] nvarchar(max)  NOT NULL,
    [Marque_ID] int  NOT NULL
);
GO

-- Creating table 'MarqueJeu'
CREATE TABLE [dbo].[MarqueJeu] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Nom] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'LogSimulationJeu'
CREATE TABLE [dbo].[LogSimulationJeu] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Date] datetime  NOT NULL,
    [Texte] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID] in table 'GaragisteJeu'
ALTER TABLE [dbo].[GaragisteJeu]
ADD CONSTRAINT [PK_GaragisteJeu]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'VoitureJeu'
ALTER TABLE [dbo].[VoitureJeu]
ADD CONSTRAINT [PK_VoitureJeu]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'PeriodeFermetureJeu'
ALTER TABLE [dbo].[PeriodeFermetureJeu]
ADD CONSTRAINT [PK_PeriodeFermetureJeu]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'RevisionJeu'
ALTER TABLE [dbo].[RevisionJeu]
ADD CONSTRAINT [PK_RevisionJeu]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'FranchiseJeu'
ALTER TABLE [dbo].[FranchiseJeu]
ADD CONSTRAINT [PK_FranchiseJeu]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ReparationJeu'
ALTER TABLE [dbo].[ReparationJeu]
ADD CONSTRAINT [PK_ReparationJeu]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ModeleJeu'
ALTER TABLE [dbo].[ModeleJeu]
ADD CONSTRAINT [PK_ModeleJeu]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'MarqueJeu'
ALTER TABLE [dbo].[MarqueJeu]
ADD CONSTRAINT [PK_MarqueJeu]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'LogSimulationJeu'
ALTER TABLE [dbo].[LogSimulationJeu]
ADD CONSTRAINT [PK_LogSimulationJeu]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Garagiste_ID] in table 'PeriodeFermetureJeu'
ALTER TABLE [dbo].[PeriodeFermetureJeu]
ADD CONSTRAINT [FK_GaragistePeriodeFermeture]
    FOREIGN KEY ([Garagiste_ID])
    REFERENCES [dbo].[GaragisteJeu]
        ([ID])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_GaragistePeriodeFermeture'
CREATE INDEX [IX_FK_GaragistePeriodeFermeture]
ON [dbo].[PeriodeFermetureJeu]
    ([Garagiste_ID]);
GO

-- Creating foreign key on [Franchise_ID] in table 'GaragisteJeu'
ALTER TABLE [dbo].[GaragisteJeu]
ADD CONSTRAINT [FK_FranchiseGaragiste]
    FOREIGN KEY ([Franchise_ID])
    REFERENCES [dbo].[FranchiseJeu]
        ([ID])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_FranchiseGaragiste'
CREATE INDEX [IX_FK_FranchiseGaragiste]
ON [dbo].[GaragisteJeu]
    ([Franchise_ID]);
GO

-- Creating foreign key on [Garagiste_ID] in table 'ReparationJeu'
ALTER TABLE [dbo].[ReparationJeu]
ADD CONSTRAINT [FK_GaragisteReparation]
    FOREIGN KEY ([Garagiste_ID])
    REFERENCES [dbo].[GaragisteJeu]
        ([ID])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_GaragisteReparation'
CREATE INDEX [IX_FK_GaragisteReparation]
ON [dbo].[ReparationJeu]
    ([Garagiste_ID]);
GO

-- Creating foreign key on [Voiture_ID] in table 'ReparationJeu'
ALTER TABLE [dbo].[ReparationJeu]
ADD CONSTRAINT [FK_VoitureReparation]
    FOREIGN KEY ([Voiture_ID])
    REFERENCES [dbo].[VoitureJeu]
        ([ID])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_VoitureReparation'
CREATE INDEX [IX_FK_VoitureReparation]
ON [dbo].[ReparationJeu]
    ([Voiture_ID]);
GO

-- Creating foreign key on [Modele_ID] in table 'VoitureJeu'
ALTER TABLE [dbo].[VoitureJeu]
ADD CONSTRAINT [FK_ModeleVoiture]
    FOREIGN KEY ([Modele_ID])
    REFERENCES [dbo].[ModeleJeu]
        ([ID])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ModeleVoiture'
CREATE INDEX [IX_FK_ModeleVoiture]
ON [dbo].[VoitureJeu]
    ([Modele_ID]);
GO

-- Creating foreign key on [Marque_ID] in table 'ModeleJeu'
ALTER TABLE [dbo].[ModeleJeu]
ADD CONSTRAINT [FK_MarqueModele]
    FOREIGN KEY ([Marque_ID])
    REFERENCES [dbo].[MarqueJeu]
        ([ID])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_MarqueModele'
CREATE INDEX [IX_FK_MarqueModele]
ON [dbo].[ModeleJeu]
    ([Marque_ID]);
GO

-- Creating foreign key on [Modele_ID] in table 'RevisionJeu'
ALTER TABLE [dbo].[RevisionJeu]
ADD CONSTRAINT [FK_ModeleRevision]
    FOREIGN KEY ([Modele_ID])
    REFERENCES [dbo].[ModeleJeu]
        ([ID])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ModeleRevision'
CREATE INDEX [IX_FK_ModeleRevision]
ON [dbo].[RevisionJeu]
    ([Modele_ID]);
GO

-- Creating foreign key on [Marque_ID] in table 'RevisionJeu'
ALTER TABLE [dbo].[RevisionJeu]
ADD CONSTRAINT [FK_MarqueRevision]
    FOREIGN KEY ([Marque_ID])
    REFERENCES [dbo].[MarqueJeu]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_MarqueRevision'
CREATE INDEX [IX_FK_MarqueRevision]
ON [dbo].[RevisionJeu]
    ([Marque_ID]);
GO

-- Creating foreign key on [Revision_ID] in table 'ReparationJeu'
ALTER TABLE [dbo].[ReparationJeu]
ADD CONSTRAINT [FK_ReparationRevision]
    FOREIGN KEY ([Revision_ID])
    REFERENCES [dbo].[RevisionJeu]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ReparationRevision'
CREATE INDEX [IX_FK_ReparationRevision]
ON [dbo].[ReparationJeu]
    ([Revision_ID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------