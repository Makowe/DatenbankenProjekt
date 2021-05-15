USE project_9275184;

INSERT INTO recipe (name, people) VALUES ('Paniertes Schnitzel mit Pommes',4);
INSERT INTO recipe (name, people) VALUES ('Rösti',4);

INSERT INTO component (name) VALUES ('Schweineschnitzel');
INSERT INTO component (name) VALUES ('Pommes');
INSERT INTO component (name) VALUES ('Paniermehl');
INSERT INTO component (name) VALUES ('Pfeffer');
INSERT INTO component (name) VALUES ('Salz');
INSERT INTO component (name) VALUES ('Kartoffeln');
INSERT INTO component (name) VALUES ('Zwiebel');
INSERT INTO component (name) VALUES ('Paprikagewürz');
INSERT INTO component (name) VALUES ('Eier');

INSERT INTO unit (name,shortname) VALUES ('Gramm','g');
INSERT INTO unit (name,shortname) VALUES ('Stück','St.');
INSERT INTO unit (name,shortname) VALUES ('Milliliter','ml');
INSERT INTO unit (name,shortname) VALUES ('Esslöffel','El.');
INSERT INTO unit (name,shortname) VALUES ('Teelöffel','Tl.');
INSERT INTO unit (name,shortname) VALUES ('Prise','Pr.');

INSERT INTO component_in_recipe (component, recipe, amount, unit) VALUES (1,1,800,1);
INSERT INTO component_in_recipe (component, recipe, amount, unit) VALUES (2,1,1000,1);
INSERT INTO component_in_recipe (component, recipe, amount, unit) VALUES (3,1,200,1);
INSERT INTO component_in_recipe (component, recipe, amount, unit) VALUES (4,1,1,6);
INSERT INTO component_in_recipe (component, recipe, amount, unit) VALUES (5,1,1,6);
INSERT INTO component_in_recipe (component, recipe, amount, unit) VALUES (8,1,10,1);
INSERT INTO component_in_recipe (component, recipe, amount, unit) VALUES (9,1,2,2);
INSERT INTO component_in_recipe (component, recipe, amount, unit) VALUES (6,2,2,2);
INSERT INTO component_in_recipe (component, recipe, amount, unit) VALUES (7,2,2,2);
INSERT INTO component_in_recipe (component, recipe, amount, unit) VALUES (5,2,10,1);

INSERT INTO instruction (recipe, step, description) VALUES (1,1,'Schnitzel klopfen, würzen und panieren');
INSERT INTO instruction (recipe, step, description) VALUES (1,2,'Pommes fritieren');
INSERT INTO instruction (recipe, step, description) VALUES (1,3,'Schnitzel braten');
INSERT INTO instruction (recipe, step, description) VALUES (2,1,'Kartoffeln reiben');
INSERT INTO instruction (recipe, step, description) VALUES (2,2,'Zwiebeln hacken');
INSERT INTO instruction (recipe, step, description) VALUES (2,3,'Kartoffeln und Zwiebeln vermischen und würzen');
INSERT INTO instruction (recipe, step, description) VALUES (2,4,'Die Masse braten');
