
-- Add MODEL3DTHREEJS as item view type

if ( not exists ( select 1 from SobekCM_Item_Viewer_Types where ViewType = 'MODEL3DTHREEJS' ))
begin
	
	insert into SobekCM_Item_Viewer_Types ( ViewType, [Order], DefaultView, MenuOrder )
	values ( 'MODEL3DTHREEJS', 1, 0, 97 );

end;
GO
