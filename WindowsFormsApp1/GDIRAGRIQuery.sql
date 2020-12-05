create table societe
( id_societe int PRIMARY KEY identity,
nom VARCHAR  (50),
genre varchar(20) not null check (genre in ('client','fournisseur'))
);

create table facture 
( num_facture varchar(50) PRIMARY KEY ,
mt_tva decimal(12,3),
mt_ttc decimal(12,3)
);

create table banque
(
id_banque int PRIMARY KEY identity,
nom_bq varchar(50),
taux decimal(12,3),
);

create table utilisateur
(
id_utilisateur int PRIMARY KEY identity,
username varchar(20), 
pass varchar(20)
);

create table droits
(
id_utilisateur int PRIMARY KEY,
saisie bit,
consultation bit,
gestion_cmpt bit
);

alter table droits add foreign key (id_utilisateur) references utilisateur(id_utilisateur) 
ON DELETE CASCADE ON UPDATE CASCADE ;

create table operation
(id_operation int PRIMARY KEY identity ,
date_ech date,
date_remise date,
date_encaissement date,
date_saisie date default GETDATE(),
nature varchar(50),
num_regl varchar(50),
mode_regl varchar(50),
num_facture varchar(50) FOREIGN KEY REFERENCES facture (num_facture),
num_remise varchar(50),
id_societe int FOREIGN KEY REFERENCES societe,
id_utilisateur int FOREIGN KEY REFERENCES utilisateur, 
banque varchar (50),
id_banque int FOREIGN KEY REFERENCES banque (id_banque), 
montant decimal(12,3),
situation varchar (100),
observation varchar(150)
);

create table effet
(
id_effet int PRIMARY KEY identity,
id_operation int FOREIGN KEY REFERENCES operation,
type_eff varchar(50) not null check (type_eff in ('encaissement','escompte'))
);


ALTER TABLE effet
ADD taux  decimal(12,3)
 

 
ALTER TABLE banque
ADD UNIQUE (nom_bq); 

ALTER TABLE societe
ADD UNIQUE (nom); 

ALTER TABLE utilisateur
ADD UNIQUE (username); 

ALTER TABLE effet
ADD UNIQUE (id_operation); 



insert into societe values ('yassine','client');
insert into banque values ('BP','50');
insert into facture values ('2222',121212.33,13133.44);
insert into utilisateur values ('hassan','123123');
insert into droits values (1,1,1,1);
insert into operation(date_ech,date_remise,date_encaissement,nature,num_regl,mode_regl,num_facture,num_remise,id_societe,id_utilisateur,banque,id_banque,montant,situation,observation)
values ('2018-01-13','2018-01-13','2018-01-13','Caisse','1111','EFFF','2222','3333',1,1,'CIH',1,12233340.11,'sssss','ooooo');
insert into effet values (1,'encaissement',122.22);
insert into effet values (2,'escompte');




select * from operation;
select * from banque;
select * from societe;
select * from facture;
select * from effet;
select * from utilisateur;
select * from droits;
 
 delete from utilisateur where id_utilisateur = 1;

drop table effet;
drop table operation;
drop table banque;
drop table facture;
drop table societe;
drop table droits;
drop table utilisateur;



/*create trigger TRG1
on utilisateur
instead of delete
as
begin
declare @id int ;
set @id = (select id_utilisateur from deleted);

delete from droits where id_utilisateur=@id;
delete from utilisateur where id_utilisateur=@id;

end

drop trigger  TRG1*/

create trigger effet_after_insert on operation after insert
as

DECLARE @var1 INT
DECLARE @id_bq INT
DECLARE @taux_banque decimal(12,3)
DECLARE @var2 varchar(20)
 
    SET @var1 = (select id_operation from inserted)
	SET @var2 = (select mode_regl from inserted)
	SET @id_bq = (select id_banque from inserted where id_operation=@var1)
	SET @taux_banque = (select taux from banque where id_banque=@id_bq)
if(@var2 = 'EFF')
begin
insert into effet values (@var1 ,'encaissement', @taux_banque)
end

drop trigger effet_after_insert



create trigger trigger_delete_op
on operation
Instead of delete
as
begin
declare @id int;
set @id = (select id_operation from deleted)

begin tran
delete from effet
where id_operation=@id
delete from operation 
where id_operation=@id
Commit tran
end






select O.id_operation, O.date_ech,O.date_remise, O.mode_regl , O.banque, O.num_regl, S.nom  , O.num_remise, O.nature,O.montant , B.nom_bq As BQ_remise 
from  operation O Left join banque B On O.id_banque=B.id_banque , societe S  
where O.id_societe=S.id_societe 

