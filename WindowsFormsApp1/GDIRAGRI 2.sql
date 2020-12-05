select * from operation
select * from effet



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

