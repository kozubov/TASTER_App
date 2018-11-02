
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 09/19/2018 22:12:15
-- Generated from EDMX file: D:\git\Kursak\Tests_Project\Kursak_Ol\Tests_DB.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Tests_DB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_UserRole]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[User] DROP CONSTRAINT [FK_UserRole];
GO
IF OBJECT_ID(N'[dbo].[FK_TestCategory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Test] DROP CONSTRAINT [FK_TestCategory];
GO
IF OBJECT_ID(N'[dbo].[FK_TestQuestionTest]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TestQuestion] DROP CONSTRAINT [FK_TestQuestionTest];
GO
IF OBJECT_ID(N'[dbo].[FK_TestQuestionAnswerTestQuestion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TestQuestionAnswer] DROP CONSTRAINT [FK_TestQuestionAnswerTestQuestion];
GO
IF OBJECT_ID(N'[dbo].[FK_UserTestCreator]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TestCreator] DROP CONSTRAINT [FK_UserTestCreator];
GO
IF OBJECT_ID(N'[dbo].[FK_TestTestCreator]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TestCreator] DROP CONSTRAINT [FK_TestTestCreator];
GO
IF OBJECT_ID(N'[dbo].[FK_UserTestUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserTest] DROP CONSTRAINT [FK_UserTestUser];
GO
IF OBJECT_ID(N'[dbo].[FK_UserTestTest]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserTest] DROP CONSTRAINT [FK_UserTestTest];
GO
IF OBJECT_ID(N'[dbo].[FK_UserTestAnswerUserTest]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserTestAnswer] DROP CONSTRAINT [FK_UserTestAnswerUserTest];
GO
IF OBJECT_ID(N'[dbo].[FK_UserTestAnswerTestQuestion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserTestAnswer] DROP CONSTRAINT [FK_UserTestAnswerTestQuestion];
GO
IF OBJECT_ID(N'[dbo].[FK_TestQuestionAnswerUserTestAnswer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserTestAnswer] DROP CONSTRAINT [FK_TestQuestionAnswerUserTestAnswer];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Role]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Role];
GO
IF OBJECT_ID(N'[dbo].[User]', 'U') IS NOT NULL
    DROP TABLE [dbo].[User];
GO
IF OBJECT_ID(N'[dbo].[Category]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Category];
GO
IF OBJECT_ID(N'[dbo].[Test]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Test];
GO
IF OBJECT_ID(N'[dbo].[TestCreator]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TestCreator];
GO
IF OBJECT_ID(N'[dbo].[TestQuestion]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TestQuestion];
GO
IF OBJECT_ID(N'[dbo].[TestQuestionAnswer]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TestQuestionAnswer];
GO
IF OBJECT_ID(N'[dbo].[UserTest]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserTest];
GO
IF OBJECT_ID(N'[dbo].[UserTestAnswer]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserTestAnswer];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Role'
CREATE TABLE [dbo].[Role] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(30)  NOT NULL
);
GO

-- Creating table 'User'
CREATE TABLE [dbo].[User] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Login] nvarchar(30)  NOT NULL,
    [Password] nvarchar(255)  NOT NULL,
    [LastName] nvarchar(30)  NOT NULL,
    [FirstName] nvarchar(30)  NOT NULL,
    [MiddleName] nvarchar(30)  NOT NULL,
    [Phone] nchar(12)  NOT NULL,
    [Address] nvarchar(255)  NOT NULL,
    [RoleId] int  NOT NULL
);
GO

-- Creating table 'Category'
CREATE TABLE [dbo].[Category] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(30)  NOT NULL
);
GO

-- Creating table 'Test'
CREATE TABLE [dbo].[Test] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(70)  NOT NULL,
    [IsActual] tinyint  NOT NULL,
    [CategoryId] int  NOT NULL
);
GO

-- Creating table 'TestCreator'
CREATE TABLE [dbo].[TestCreator] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] int  NOT NULL,
    [TestId] int  NOT NULL
);
GO

-- Creating table 'TestQuestion'
CREATE TABLE [dbo].[TestQuestion] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Question] nvarchar(max)  NOT NULL,
    [IsActual] tinyint  NOT NULL,
    [TestId] int  NOT NULL
);
GO

-- Creating table 'TestQuestionAnswer'
CREATE TABLE [dbo].[TestQuestionAnswer] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Answer] nvarchar(255)  NOT NULL,
    [IsAnswer] tinyint  NOT NULL,
    [TestQuestionId] int  NOT NULL
);
GO

-- Creating table 'UserTest'
CREATE TABLE [dbo].[UserTest] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] int  NOT NULL,
    [TestId] int  NOT NULL,
    [Result] nvarchar(max)  NOT NULL,
    [StartDate] datetime  NOT NULL,
    [EndDate] datetime  NOT NULL
);
GO

-- Creating table 'UserTestAnswer'
CREATE TABLE [dbo].[UserTestAnswer] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserTestId] int  NOT NULL,
    [TestQuestionId] int  NOT NULL,
    [UserTestQuestionAnswerId] int  NOT NULL,
    [AnswerDate] datetime  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Role'
ALTER TABLE [dbo].[Role]
ADD CONSTRAINT [PK_Role]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'User'
ALTER TABLE [dbo].[User]
ADD CONSTRAINT [PK_User]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Category'
ALTER TABLE [dbo].[Category]
ADD CONSTRAINT [PK_Category]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Test'
ALTER TABLE [dbo].[Test]
ADD CONSTRAINT [PK_Test]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TestCreator'
ALTER TABLE [dbo].[TestCreator]
ADD CONSTRAINT [PK_TestCreator]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TestQuestion'
ALTER TABLE [dbo].[TestQuestion]
ADD CONSTRAINT [PK_TestQuestion]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TestQuestionAnswer'
ALTER TABLE [dbo].[TestQuestionAnswer]
ADD CONSTRAINT [PK_TestQuestionAnswer]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserTest'
ALTER TABLE [dbo].[UserTest]
ADD CONSTRAINT [PK_UserTest]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserTestAnswer'
ALTER TABLE [dbo].[UserTestAnswer]
ADD CONSTRAINT [PK_UserTestAnswer]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [RoleId] in table 'User'
ALTER TABLE [dbo].[User]
ADD CONSTRAINT [FK_UserRole]
    FOREIGN KEY ([RoleId])
    REFERENCES [dbo].[Role]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserRole'
CREATE INDEX [IX_FK_UserRole]
ON [dbo].[User]
    ([RoleId]);
GO

-- Creating foreign key on [CategoryId] in table 'Test'
ALTER TABLE [dbo].[Test]
ADD CONSTRAINT [FK_TestCategory]
    FOREIGN KEY ([CategoryId])
    REFERENCES [dbo].[Category]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TestCategory'
CREATE INDEX [IX_FK_TestCategory]
ON [dbo].[Test]
    ([CategoryId]);
GO

-- Creating foreign key on [TestId] in table 'TestQuestion'
ALTER TABLE [dbo].[TestQuestion]
ADD CONSTRAINT [FK_TestQuestionTest]
    FOREIGN KEY ([TestId])
    REFERENCES [dbo].[Test]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TestQuestionTest'
CREATE INDEX [IX_FK_TestQuestionTest]
ON [dbo].[TestQuestion]
    ([TestId]);
GO

-- Creating foreign key on [TestQuestionId] in table 'TestQuestionAnswer'
ALTER TABLE [dbo].[TestQuestionAnswer]
ADD CONSTRAINT [FK_TestQuestionAnswerTestQuestion]
    FOREIGN KEY ([TestQuestionId])
    REFERENCES [dbo].[TestQuestion]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TestQuestionAnswerTestQuestion'
CREATE INDEX [IX_FK_TestQuestionAnswerTestQuestion]
ON [dbo].[TestQuestionAnswer]
    ([TestQuestionId]);
GO

-- Creating foreign key on [UserId] in table 'TestCreator'
ALTER TABLE [dbo].[TestCreator]
ADD CONSTRAINT [FK_UserTestCreator]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[User]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserTestCreator'
CREATE INDEX [IX_FK_UserTestCreator]
ON [dbo].[TestCreator]
    ([UserId]);
GO

-- Creating foreign key on [TestId] in table 'TestCreator'
ALTER TABLE [dbo].[TestCreator]
ADD CONSTRAINT [FK_TestTestCreator]
    FOREIGN KEY ([TestId])
    REFERENCES [dbo].[Test]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TestTestCreator'
CREATE INDEX [IX_FK_TestTestCreator]
ON [dbo].[TestCreator]
    ([TestId]);
GO

-- Creating foreign key on [UserId] in table 'UserTest'
ALTER TABLE [dbo].[UserTest]
ADD CONSTRAINT [FK_UserTestUser]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[User]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserTestUser'
CREATE INDEX [IX_FK_UserTestUser]
ON [dbo].[UserTest]
    ([UserId]);
GO

-- Creating foreign key on [TestId] in table 'UserTest'
ALTER TABLE [dbo].[UserTest]
ADD CONSTRAINT [FK_UserTestTest]
    FOREIGN KEY ([TestId])
    REFERENCES [dbo].[Test]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserTestTest'
CREATE INDEX [IX_FK_UserTestTest]
ON [dbo].[UserTest]
    ([TestId]);
GO

-- Creating foreign key on [UserTestId] in table 'UserTestAnswer'
ALTER TABLE [dbo].[UserTestAnswer]
ADD CONSTRAINT [FK_UserTestAnswerUserTest]
    FOREIGN KEY ([UserTestId])
    REFERENCES [dbo].[UserTest]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserTestAnswerUserTest'
CREATE INDEX [IX_FK_UserTestAnswerUserTest]
ON [dbo].[UserTestAnswer]
    ([UserTestId]);
GO

-- Creating foreign key on [TestQuestionId] in table 'UserTestAnswer'
ALTER TABLE [dbo].[UserTestAnswer]
ADD CONSTRAINT [FK_UserTestAnswerTestQuestion]
    FOREIGN KEY ([TestQuestionId])
    REFERENCES [dbo].[TestQuestion]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserTestAnswerTestQuestion'
CREATE INDEX [IX_FK_UserTestAnswerTestQuestion]
ON [dbo].[UserTestAnswer]
    ([TestQuestionId]);
GO

-- Creating foreign key on [UserTestQuestionAnswerId] in table 'UserTestAnswer'
ALTER TABLE [dbo].[UserTestAnswer]
ADD CONSTRAINT [FK_TestQuestionAnswerUserTestAnswer]
    FOREIGN KEY ([UserTestQuestionAnswerId])
    REFERENCES [dbo].[TestQuestionAnswer]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TestQuestionAnswerUserTestAnswer'
CREATE INDEX [IX_FK_TestQuestionAnswerUserTestAnswer]
ON [dbo].[UserTestAnswer]
    ([UserTestQuestionAnswerId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------

INSERT INTO dbo.[Role] VALUES
('Преподаватель'),
('Студент');
GO

INSERT INTO dbo.[User] VALUES
('teacher', '1811971554', 'Захимию', 'Антонина', 'Карповна', '380283485868', 'Химическая лаборатория 123', 1),
('teacher2', '1811971554', 'Зафизику', 'Вениамин', 'Баракович', '380876545432', 'Физическая лаборатория 1', 1),
('user', '159167266', 'Маоузаров', 'Валериан', 'Унагович', '380554443322', 'Тут адреса нет, д. 54', 2);
GO

INSERT INTO dbo.[Category] VALUES
('Физика'),
('Математика'),
('Химия');
GO

INSERT INTO dbo.[Test] VALUES
('Тест по физике №1', 1, 1),
('Тест по математике №1', 1, 2),
('Тест по химии №1', 1, 3);
GO

INSERT INTO dbo.[TestCreator] VALUES
(2,1),
(1,2),
(1,3);
GO

INSERT INTO dbo.[TestQuestion] VALUES
('Закон Ома для участка цепи', 1, 1),
('Закон Кулона описывает взаимодействие', 1,1),
('Квадрат гипотенузы равен', 1,2),
('5!!=', 1,2),
('Формула закиси азота(веселящего газа)', 1,3),
('При взаимодействии активного металла с водой образуется', 1,3),
('За два периода полураспада распадутся ', 1, 1),
('При уменьшении в 2 раза расстояния между обкладками плоского конденсатора емкость: ', 1, 1),
('Плазма является: ', 1, 1),
('Позитрон является', 1, 1),
('При аннигиляции электрон-позитронной пары образуются', 1, 1),
('Перечисленный ряд 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79,  является', 1, 2),
('Площадь круга равна', 1, 2),
('Сумма углов треугольника равна', 1, 2),
('Кубический корень из 512 равен', 1, 2),
('Катализатор  -- это вещество которое', 1, 3),
('Йод является индикатором для каких веществ', 1, 3);
GO

INSERT INTO dbo.[TestQuestionAnswer] VALUES
('Ток прямо пропорционален напряжению и обратно пропорционален сопротивлению', 1,1),
('Емкость прямо пропорционален напряжению и обратно пропорционален сопротивлению', 0,1),
('Индуктивность обратно пропорционален напряжению и прямо пропорциональна сопротивлению', 0,1),
('Между неподвижными точечными электрическими зарядами', 1,2),
('Между неподвижными точечными магнитными зарядами', 0,2),
('Сумме квадратов катетов', 1,3),
('Квадрату суммы катетов', 0,3),
('15', 1,4),
('120', 0,4),
('N2O', 1,5),
('NO', 0,5),
('NO2', 0,5),
('NH3', 0,5),
('NH3', 0,5),
('Щелочь', 1,6),
('Кислота', 0,6),
('Соль активного металла', 0,6),
('100% радиоактивных изотопов', 0,7),
('75% радиоактивных изотопов', 1,7),
('0% радиоактивных изотопов', 0,7),
('25% радиоактивных изотопов', 0,7),
('уменьшится в 2 раза', 0,8),
('увеличится в 2 раза', 1,8),
('останется неизменной', 0,8),
('феромагнетиком', 0,9),
('парамагнетиком', 1,9),
('диамагнетиком', 0,9),
('барионом', 0,10),
('лептоном', 1,10),
('бозоном', 0,10),
('мезоны', 0,11),
('барионы', 0,11),
('фотоны', 1,11),
('лептоном', 1,11),
('натуральными числами', 0,12),
('рядом Фибоначчи', 0,12),
('простыми числами', 1,12),
('2*Pi*R', 0,13),
('Pi*R^2', 1,13),
('360 градусов', 0,14),
('90 градусов', 0,14),
('120 градусов', 0,14),
('180 градусов', 0,14),
('64', 0,15),
('32', 0,15),
('128', 0,15),
('4', 0,15),
('8', 1,15),
('не оказывающее влияния на скорость реакции', 0,16),
('многократно замедляет реакцию', 0,16),
('многократно ускоряет реакцию', 1,16),
('белков', 0,17),
('жиров', 0,17),
('витаминов', 0,17),
('крахмалов', 1,17);
GO