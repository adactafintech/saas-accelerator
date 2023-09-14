select *
from dbo.Subscriptions
where AMPSubscriptionId = '78031411-a293-4113-c769-98afc849f433'

select *
from dbo.Offers

select *
from dbo.Plans
where OfferID = '955323a5-05cf-4ef8-b07e-7c848e5f7020'

select *
from [dbo].[OfferAttributes]

select *
from SubscriptionAttributeValues
where PlanID = '5c0ac747-5d69-49d4-85a5-2ed4bace2c09'
  and PlanAttributeId = 1
  and [Value] = 'Test01.2225'

-- update dbo.Subscriptions
-- set SubscriptionStatus = 'Deprovisioned'
-- where Id = 8

-- update SubscriptionAttributeValues
-- set Value = 'igor-saas-01'
-- where ID = 1

select *
from ApplicationLog
order by ActionTime desc

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
