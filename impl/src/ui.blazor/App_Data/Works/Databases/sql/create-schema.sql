
    PRAGMA foreign_keys = OFF

    drop table if exists departamento

    drop table if exists pessoa

    PRAGMA foreign_keys = ON

    create table departamento (
        id  integer primary key autoincrement,
       nome TEXT
    )

    create table pessoa (
        id  integer primary key autoincrement,
       nome TEXT,
       salario REAL not null,
       departamento_id INTEGER,
       constraint FK_Pessoa_Departamento foreign key (departamento_id) references departamento
    )

    create index PESSOA_PESSOA_DEPARTAMENTO_ID_FK_IDX on pessoa (departamento_id)
