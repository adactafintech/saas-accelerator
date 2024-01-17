/*** BACKUP ALL CHANGED TABLES *************************************/

select *
into dbo.SubscriptionsBkp
from dbo.Subscriptions

select *
into dbo.OffersBkp
from dbo.Offers

select *
into dbo.PlansBkp
from dbo.Plans

select *
into dbo.OfferAttributesBkp
from dbo.OfferAttributes

select *
into dbo.PlanAttributeMappingBkp
from dbo.PlanAttributeMapping

select *
into dbo.SubscriptionAttributeValuesBkp
from dbo.SubscriptionAttributeValues

/*** DELETE BACKUPS *************************************/


/*** CLEANUP COMMANDS *************************************/

-- DELETE from SubscriptionAuditLogs
-- where SubscriptionID in (1, 2)

-- DELETE from Subscriptions
-- where AMPSubscriptionId IN (
--            'e313a458-a498-4dd4-c659-0aa578671dd2',
--            'ce7d2d33-bc94-4ccb-d1e8-3f84e517acfd')

-- delete from Offers
-- where Id = 1

-- delete from Plans
-- where Id in (3, 4)

-- update Plans
-- set OfferID = 'e0119735-f051-4f44-a642-4c99d090e139'

-- delete from OfferAttributes
-- where OfferId = '0e254ee7-1bb4-4003-8f83-5408940db069'

-- update PlanAttributeMapping
-- set OfferAttributeID = OfferAttributeID + 9

-- delete
-- from SubscriptionAttributeValues
-- where SubscriptionId IN (
--            'e313a458-a498-4dd4-c659-0aa578671dd2',
--            'ce7d2d33-bc94-4ccb-d1e8-3f84e517acfd')

-- update SubscriptionAttributeValues
-- set PlanAttributeId = PlanAttributeId - 27

-- delete
-- from WebJobSubscriptionStatus
-- where SubscriptionId IN (
--            'e313a458-a498-4dd4-c659-0aa578671dd2',
--            'ce7d2d33-bc94-4ccb-d1e8-3f84e517acfd')

/*** ANALYSIS COMMANDS *************************************/

select *
from SubscriptionAuditLogs
where SubscriptionID in (1, 2)

select *
from dbo.Subscriptions
where AMPSubscriptionId IN (
           'e313a458-a498-4dd4-c659-0aa578671dd2',
           'ce7d2d33-bc94-4ccb-d1e8-3f84e517acfd')

select *
from dbo.Offers

-- update Plans
-- set OfferID = '0e254ee7-1bb4-4003-8f83-5408940db069'

select *
from dbo.Plans
where OfferID = '0e254ee7-1bb4-4003-8f83-5408940db069'

select *
from [dbo].[OfferAttributes]

select *
from SubscriptionAttributeValues
where PlanID = '5c0ac747-5d69-49d4-85a5-2ed4bace2c09'
  and PlanAttributeId = 1
  and [Value] = 'Test01.2225'

-- update dbo.Subscriptions
-- set SubscriptionStatus = 'Deprovisioned'
-- where Id = 7

-- update SubscriptionAttributeValues
-- set Value = 'igor-saas-01'
-- where ID = 1

select *
from ApplicationLog
order by ActionTime desc

select *
from [dbo].[OfferAttributes]
where OfferId = '0e254ee7-1bb4-4003-8f83-5408940db069' -- 'e0119735-f051-4f44-a642-4c99d090e139'


select *
from PlanAttributeMapping
where PlanId = '258d850a-2d9c-4ea0-9ea5-ac2cf0159970'

select OfferId, count(*)
from OfferAttributes
--where --OfferId='0e254ee7-1bb4-4003-8f83-5408940db069'
--id = 11
group by OfferId

select *
from OfferAttributes
where OfferId = 'e0119735-f051-4f44-a642-4c99d090e139'

--Select OfferId from Plans where PlanGuId ='258d850a-2d9c-4ea0-9ea5-ac2cf0159970'

/**** ANALYZE PLAN ATTRIBUTE MAPPING ********************************************************/
select *
from Plans

select PlanId, count(*)
from PlanAttributeMapping
group by PlanId

select *
from PlanAttributeMapping
--where OfferAttributeID BETWEEN 1 and 9
where OfferAttributeID BETWEEN 10 and 18
order by PlanAttributeId, OfferAttributeID


-- delete
-- from PlanAttributeMapping
-- where PlanAttributeId = 1

-- SET IDENTITY_INSERT dbo.PlanAttributeMapping ON

-- insert into PlanAttributeMapping
-- (PlanAttributeId, PlanId, OfferAttributeID, IsEnabled, CreateDate, UserId)
-- values (1, '258d850a-2d9c-4ea0-9ea5-ac2cf0159970', 1, 1, GETDATE(), 1)

-- insert into PlanAttributeMapping
-- (PlanAttributeId, PlanId, OfferAttributeID, IsEnabled, CreateDate, UserId)
-- values (2, '258d850a-2d9c-4ea0-9ea5-ac2cf0159970', 2, 1, GETDATE(), 1)

-- insert into PlanAttributeMapping
-- (PlanAttributeId, PlanId, OfferAttributeID, IsEnabled, CreateDate, UserId)
-- values (3, '258d850a-2d9c-4ea0-9ea5-ac2cf0159970', 3, 1, GETDATE(), 1)

-- insert into PlanAttributeMapping
-- (PlanAttributeId, PlanId, OfferAttributeID, IsEnabled, CreateDate, UserId)
-- values (4, '258d850a-2d9c-4ea0-9ea5-ac2cf0159970', 4, 1, GETDATE(), 1)

-- insert into PlanAttributeMapping
-- (PlanAttributeId, PlanId, OfferAttributeID, IsEnabled, CreateDate, UserId)
-- values (5, '258d850a-2d9c-4ea0-9ea5-ac2cf0159970', 5, 1, GETDATE(), 1)

-- insert into PlanAttributeMapping
-- (PlanAttributeId, PlanId, OfferAttributeID, IsEnabled, CreateDate, UserId)
-- values (6, '258d850a-2d9c-4ea0-9ea5-ac2cf0159970', 6, 1, GETDATE(), 1)

-- insert into PlanAttributeMapping
-- (PlanAttributeId, PlanId, OfferAttributeID, IsEnabled, CreateDate, UserId)
-- values (7, '258d850a-2d9c-4ea0-9ea5-ac2cf0159970', 7, 1, GETDATE(), 1)

-- insert into PlanAttributeMapping
-- (PlanAttributeId, PlanId, OfferAttributeID, IsEnabled, CreateDate, UserId)
-- values (8, '258d850a-2d9c-4ea0-9ea5-ac2cf0159970', 8, 1, GETDATE(), 1)

-- insert into PlanAttributeMapping
-- (PlanAttributeId, PlanId, OfferAttributeID, IsEnabled, CreateDate, UserId)
-- values (9, '258d850a-2d9c-4ea0-9ea5-ac2cf0159970', 9, 1, GETDATE(), 1)

-- insert into PlanAttributeMapping
-- (PlanAttributeId, PlanId, OfferAttributeID, IsEnabled, CreateDate, UserId)
-- values (10, '2de12ef9-5708-4c31-af04-2dfbf9057d8c', 10, 1, GETDATE(), 1)

-- insert into PlanAttributeMapping
-- (PlanAttributeId, PlanId, OfferAttributeID, IsEnabled, CreateDate, UserId)
-- values (11, '2de12ef9-5708-4c31-af04-2dfbf9057d8c', 11, 1, GETDATE(), 1)

-- insert into PlanAttributeMapping
-- (PlanAttributeId, PlanId, OfferAttributeID, IsEnabled, CreateDate, UserId)
-- values (12, '2de12ef9-5708-4c31-af04-2dfbf9057d8c', 12, 1, GETDATE(), 1)

-- insert into PlanAttributeMapping
-- (PlanAttributeId, PlanId, OfferAttributeID, IsEnabled, CreateDate, UserId)
-- values (13, '2de12ef9-5708-4c31-af04-2dfbf9057d8c', 13, 1, GETDATE(), 1)

-- insert into PlanAttributeMapping
-- (PlanAttributeId, PlanId, OfferAttributeID, IsEnabled, CreateDate, UserId)
-- values (14, '2de12ef9-5708-4c31-af04-2dfbf9057d8c', 14, 1, GETDATE(), 1)

-- insert into PlanAttributeMapping
-- (PlanAttributeId, PlanId, OfferAttributeID, IsEnabled, CreateDate, UserId)
-- values (15, '2de12ef9-5708-4c31-af04-2dfbf9057d8c', 15, 1, GETDATE(), 1)

-- insert into PlanAttributeMapping
-- (PlanAttributeId, PlanId, OfferAttributeID, IsEnabled, CreateDate, UserId)
-- values (16, '2de12ef9-5708-4c31-af04-2dfbf9057d8c', 16, 1, GETDATE(), 1)

-- insert into PlanAttributeMapping
-- (PlanAttributeId, PlanId, OfferAttributeID, IsEnabled, CreateDate, UserId)
-- values (17, '2de12ef9-5708-4c31-af04-2dfbf9057d8c', 17, 1, GETDATE(), 1)

-- insert into PlanAttributeMapping
-- (PlanAttributeId, PlanId, OfferAttributeID, IsEnabled, CreateDate, UserId)
-- values (18, '2de12ef9-5708-4c31-af04-2dfbf9057d8c', 18, 1, GETDATE(), 1)

-- SET IDENTITY_INSERT dbo.PlanAttributeMapping OFF

select *
from PlanAttributeMapping

-- update PlanAttributeMapping
-- set OfferAttributeID = OfferAttributeID - 9
-- where PlanId = '2de12ef9-5708-4c31-af04-2dfbf9057d8c'


select *
from PlanAttributeMapping
where PlanId = '2de12ef9-5708-4c31-af04-2dfbf9057d8c'

select *
from OfferAttributes oa
-- join PlanAttributeMapping pa
--   on OA.ID= PA.OfferAttributeID and OA.OfferId='0e254ee7-1bb4-4003-8f83-5408940db069'  
-- --     and  PA.PlanId='2de12ef9-5708-4c31-af04-2dfbf9057d8c'
where
   oa.OfferId = '0e254ee7-1bb4-4003-8f83-5408940db069'

-- SELECT    
--   Cast( ROW_NUMBER() OVER ( ORDER BY OA.ID) as Int)RowNumber
-- , OA.ID
-- , PA.PlanAttributeId --,isnull(PA.PlanAttributeId,0) PlanAttributeId  
-- ,ISNULL(PA.PlanId,'258d850a-2d9c-4ea0-9ea5-ac2cf0159970') PlanId   
-- ,ISNULL(PA.OfferAttributeID ,OA.ID)  OfferAttributeID  
-- ,OA.DisplayName  
-- --,OA.DisplaySequence  
-- ,isnull(PA.IsEnabled,0) IsEnabled  
-- ,OA.Type
-- from [dbo].[OfferAttributes] OA  
-- left  join   
-- [dbo].[PlanAttributeMapping]  PA  
-- on OA.ID= PA.OfferAttributeID and OA.OfferId='0e254ee7-1bb4-4003-8f83-5408940db069'  
-- and  PA.PlanId='258d850a-2d9c-4ea0-9ea5-ac2cf0159970'  
-- where    
-- OA.OfferId='0e254ee7-1bb4-4003-8f83-5408940db069' AND --@OfferId
-- OA.Isactive=1 
-- order by OA.ID


SELECT    
  Cast( ROW_NUMBER() OVER ( ORDER BY OA.ID) as Int)RowNumber
, OA.ID
, PA.PlanAttributeId --,isnull(PA.PlanAttributeId,0) PlanAttributeId  
,ISNULL(PA.PlanId,'2de12ef9-5708-4c31-af04-2dfbf9057d8c') PlanId   
,ISNULL(PA.OfferAttributeID ,OA.ID)  OfferAttributeID  
,OA.DisplayName  
--,OA.DisplaySequence  
,isnull(PA.IsEnabled,0) IsEnabled  
,OA.Type
from [dbo].[OfferAttributes] OA  
left  join   
[dbo].[PlanAttributeMapping]  PA  
on OA.ID= PA.OfferAttributeID and OA.OfferId='0e254ee7-1bb4-4003-8f83-5408940db069'  
and  PA.PlanId='2de12ef9-5708-4c31-af04-2dfbf9057d8c'  
where    
OA.OfferId='0e254ee7-1bb4-4003-8f83-5408940db069' AND --@OfferId
OA.Isactive=1 
order by OA.ID

--select *
SELECT    
  Cast( ROW_NUMBER() OVER ( ORDER BY OA.ID) as Int)RowNumber  
,isnull(PA.PlanAttributeId,0) PlanAttributeId  
,ISNULL(PA.PlanId,'2de12ef9-5708-4c31-af04-2dfbf9057d8c') PlanId   
,ISNULL(PA.OfferAttributeID ,OA.ID)  OfferAttributeID  
,OA.DisplayName  
--,OA.DisplaySequence  
,isnull(PA.IsEnabled,0) IsEnabled  
,OA.Type
from   
[dbo].[OfferAttributes] OA  
Inner  join   
[dbo].[PlanAttributeMapping]  PA  
on OA.ID= PA.OfferAttributeID and OA.OfferId='0e254ee7-1bb4-4003-8f83-5408940db069'  
--and  PA.PlanId = '258d850a-2d9c-4ea0-9ea5-ac2cf0159970'
and  PA.PlanId = '2de12ef9-5708-4c31-af04-2dfbf9057d8c'



-- GET SUBSCRIPTION ATTRIBUTE VALUES
select *
from SubscriptionAttributeValues

SELECT
  Cast( ROW_NUMBER() OVER ( ORDER BY OA.ID) as Int)RowNumber  
,isnull(SAV.ID,0) ID  
,isnull(SAV.PlanAttributeId,PA.PlanAttributeId) PlanAttributeId  
,ISNULL(SAV.PlanId,'243b5338-3663-492f-8dad-81f23f2947ef') PlanId   
,ISNULL(PA.OfferAttributeID ,OA.ID)  OfferAttributeID  
,ISNULL(OA.DisplayName,'''')DisplayName  
,ISNULL(OA.Type,'''')Type  
-- ,ISNULL(VT.ValueType,'''') ValueType  
,ISnull(OA.DisplaySequence,0)DisplaySequence  
,isnull(PA.IsEnabled,0) IsEnabled  
,isnull(OA.IsRequired,0) IsRequired  
,ISNULL(Value,'''')Value  
,ISNULL(SubscriptionId,'d33a68cb-5c84-4698-d5eb-284d4d8145c1') SubscriptionId  
,ISNULL(SAV.OfferID,OA.OfferId) OfferID  
,SAV.UserId  
,SAV.CreateDate  
,ISNULL(oA.FromList,0) FromList  
,ISNULL(OA.ValuesList,'''') ValuesList  
,ISNULL(OA.Max,0) Max  
,ISNULL(OA.Min,0) Min
-- ,ISNULL(VT.HTMLType,'''') HTMLType  
from   
[dbo].[OfferAttributes] OA  
Inner  join   
[dbo].[PlanAttributeMapping]  PA  
on OA.ID= PA.OfferAttributeID and OA.OfferId='955323a5-05cf-4ef8-b07e-7c848e5f7020'  
and  PA.PlanId='243b5338-3663-492f-8dad-81f23f2947ef'

Left Join   
SubscriptionAttributeValues SAV  
on SAV.PlanAttributeId= PA.PlanAttributeId  
and SAV.SubscriptionId='d33a68cb-5c84-4698-d5eb-284d4d8145c1'
  
-- inner join ValueTypes VT  
-- ON OA.ValueTypeId=VT.ValueTypeId  
  
where    
OA.Isactive=1   
and PA.IsEnabled=1
and OA.ParameterId = 'TenantName'
and SAV.[Value] = 'Test01'


-- FIX VALUES OF SUBSCRIPTION PARAMETERS
select *
from Subscriptions

select p.Id, p.PlanId, p.PlanGUID, p.DisplayName, o.Id, o.OfferId, o.OfferGUId, o.OfferName, o.UserId
from Plans p
join Offers o
  on p.OfferID = o.OfferGUId

select *
from SubscriptionAttributeValues
where OfferID = '0e254ee7-1bb4-4003-8f83-5408940db069' -- e0119735-f051-4f44-a642-4c99d090e139
order by SubscriptionId, ID


select SubscriptionId, count(*)
from SubscriptionAttributeValues
group by SubscriptionId

update SubscriptionAttributeValues
set PlanAttributeId = 1
