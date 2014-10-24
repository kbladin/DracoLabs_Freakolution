using UnityEngine;
using System.Collections;

public class Chemicals : MonoBehaviour
{
	private Color chemicals;
	
	//Constructors
	public Chemicals()
	{
		float r = Random.Range(0f, 1f);
		float g = Random.Range(0f, 1f);
		float b = Random.Range(0f, 1f);
		chemicals = new Vector4(r,g,b,1f).normalized;
	}
	
	public Chemicals(float r, float g, float b)
	{
		chemicals = new Vector4(r,g,b,1f).normalized;	
	}
	
	//Need this when assigning to players
	public Chemicals(int playerNumber)
	{	
		switch (playerNumber)
		{
			case 0:
				chemicals = new Vector4(1f,0f,0f,1f).normalized;
				break;
			case 1:
				chemicals = new Vector4(0f,1f,0f,1f).normalized;
				break;
			case 2:
				chemicals = new Vector4(0f,0f,1f,1f).normalized;
				break;
			default:
				chemicals = new Vector4(1f,0f,0f,1f).normalized;
				break;
		}	
	}
	
	public Color getChemicals()
	{
		return chemicals;
	}
	
	public float getReaction(Chemicals targetChemicals)
	{
		return (this.chemicals.r * targetChemicals.chemicals.r + 
				this.chemicals.g * targetChemicals.chemicals.g + 
				this.chemicals.b * targetChemicals.chemicals.b);
	}
	
	public void addRedium(float alpha)
	{
		Vector4 tempChemical = this.chemicals;
		Vector4 chemicalDirection = Vector4.Project(this.chemicals, Color.red) - tempChemical;
		this.chemicals = tempChemical + chemicalDirection * alpha	;
		this.normalizeColor();
	}
	
	public void addGreenium(float alpha)
	{
		Vector4 tempChemical = this.chemicals;
		Vector4 chemicalDirection = Vector4.Project(this.chemicals, Color.green) - tempChemical;
		this.chemicals = tempChemical + chemicalDirection * alpha;
		this.normalizeColor();
	}
	
	public void addBlurine(float alpha)
	{
		Vector4 tempChemical = this.chemicals;
		Vector4 chemicalDirection = Vector4.Project(this.chemicals, Color.blue) - tempChemical;
		this.chemicals = tempChemical + chemicalDirection * alpha	;
		this.normalizeColor();
	}
	
	public void addChemical(float alpha, Color color)
	{
		Vector4 tempChemical = this.chemicals;
		Vector4 chemicalDirection = Vector4.Project(this.chemicals, color) - tempChemical;
		this.chemicals = tempChemical + chemicalDirection * alpha	;
		this.normalizeColor();
	}
	
	private void normalizeColor()
	{
		Vector4 temp = this.chemicals;
		this.chemicals = temp.normalized;
	}
	
}
