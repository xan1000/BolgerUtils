create table Animal
(
    AnimalID int identity not null,
    Name nvarchar(60) not null,
    constraint PK_Animal primary key (AnimalID)
);

insert into Animal (Name) values ('Alfie');
insert into Animal (Name) values ('Spark');
insert into Animal (Name) values ('Patch');
