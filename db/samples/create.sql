CREATE DATABASE project_9275184;
USE project_9275184;

CREATE TABLE recipe(
    id INT NOT NULL UNIQUE AUTO_INCREMENT,
    name VARCHAR(100),
    people INT,

    primary key (id)
);

CREATE TABLE component(
    id INT NOT NULL UNIQUE AUTO_INCREMENT,
    name VARCHAR(40),

    primary key (id)
);

CREATE TABLE unit(
    id INT UNIQUE NOT NULL AUTO_INCREMENT,
    name VARCHAR(20),
    shortname VARCHAR(10),

    primary key (id)
);

CREATE TABLE component_in_recipe(
    component INT,
    recipe INT,
    amount DECIMAL,
    unit INT,

    primary key (component, recipe),
    foreign key (component) references component(id),
    foreign key (recipe) references recipe(id),
    foreign key (unit) references unit(id)
);

CREATE TABLE instruction(
    recipe INT,
    step INT,
    description VARCHAR(200),

    primary key (recipe, step),
    foreign key (recipe) references recipe(id)
);