

insert into Categories values('cloths')
insert into Categories values('electronics')


insert into Products([PName],[Description],[Image],[UnitPrice],[UnitsInStock],[Status],[CategoryId]) values('T-shirt','xyz','C1P1',500,100,0,1)
insert into Products([PName],[Description],[Image],[UnitPrice],[UnitsInStock],[Status],[CategoryId]) values('Formal-shirt','abc','C1P2',700,180,0,1)
insert into Products([PName],[Description],[Image],[UnitPrice],[UnitsInStock],[Status],[CategoryId]) values('Sweatshirt','fdsv','C1P3',999,10,0,1)
insert into Products([PName],[Description],[Image],[UnitPrice],[UnitsInStock],[Status],[CategoryId]) values('iphone','fdsv','C2P1',100000,10,0,2)
insert into Products([PName],[Description],[Image],[UnitPrice],[UnitsInStock],[Status],[CategoryId]) values('Earphones','fdsv','C2P2',2999,100,0,2)
insert into Products([PName],[Description],[Image],[UnitPrice],[UnitsInStock],[Status],[CategoryId]) values('Laptop','fdsv','C1P3',40000,25,1,2)


insert into Users values('Aniket','Kale','8766594551','Baramati,Pune','admin','aniket@gmail.com','aniket@2310')
insert into Users values('Sanket','Kale','8390571799','Baramati,Pune','customer','Sanket@gmail.com','sanket@2310')
insert into Users values('Shahrukh','Khan','8890571799','Baramati,Pune','customer','SRK@gmail.com','srk@2310')
insert into Users values('Salman','Khan','8890571798','Baramati,Pune','customer','SK@gmail.com','sk@2310')

select * from Categories
select * from Products
select * from Users