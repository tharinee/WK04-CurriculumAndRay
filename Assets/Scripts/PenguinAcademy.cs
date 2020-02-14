using UntiyEngine;
using MLAgents;

public class PenguinAcademy : Academy
{

	public float FishSpeed {get; private set;}
	
	public float FeedRadius {get; private set;}

public override void IntializeAcademy(){

	FishSpeed = 0f;
	FeedRadius = 0f;

	FloatProperties.RegisteCallback("fish_speed", f =>
	{
		FishSpeed = f;
		});
	FloatProperties.RegisterCallback("feed_radius", f =>
	{
		FeedRadius = f;
		});

}
}