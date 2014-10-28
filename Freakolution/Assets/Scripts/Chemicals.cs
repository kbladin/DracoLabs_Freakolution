using UnityEngine;
using System.Collections;

public class Chemicals : MonoBehaviour
{
	private float redion;
	private float greenium;
	private float blurine;
	
	public float Redion {get{return redion;} set{redion = value;}}
	public float Greenium {get{return greenium;} set{greenium = value;}}
	public float Blurine {get{return blurine;} set{blurine = value;}}
	
	//Constructors
	public Chemicals()
	{
		redion = Random.Range(0f, 1f);
		greenium = Random.Range(0f, 1f);
		blurine = Random.Range(0f, 1f);
		this.normalizeColor();
	}
	
	public Chemicals(float r, float g, float b)
	{
		redion = r;
		greenium = g;
		blurine = b;
		this.normalizeColor();
	}
	
	//Need this when assigning to players
	public Chemicals(int playerNumber)
	{	
		switch (playerNumber)
		{
			case 0:
				redion = 1f;
				greenium = 0f;
				blurine = 0f;
				break;
			case 1:
				redion = 0f;
				greenium = 1f;
				blurine = 0f;
				break;
			case 2:
				redion = 0f;
				greenium = 0f;
				blurine = 1f;
				break;
			default:
				redion = 1f;
				greenium = 0f;
				blurine = 0f;
				break;
		}	
	}
	
	public Color getChemicals()
	{
		return new Color(redion, greenium, blurine, 1f);
	}
	public Color getChemicalsWithAlpha(float alpha)
	{	
		return new Color(redion, greenium, blurine, alpha);
	}
	
	// Dot-product
	public float getReaction(Chemicals targetChemicals)
	{
		return (this.redion * targetChemicals.redion + 
				this.greenium * targetChemicals.greenium + 
				this.blurine * targetChemicals.blurine);
	}
	
	public void addRedium(float alpha)
	{
		Vector3 tempChemical = new Vector3(redion, greenium, blurine);
		Vector3 chemicalDirection = Vector3.Project(tempChemical,new Vector3(1, 0, 0)) - tempChemical;
		tempChemical = tempChemical + chemicalDirection * alpha	;
		redion = tempChemical.x;
		greenium = tempChemical.y;
		blurine = tempChemical.z;
		this.normalizeColor();
	}
	
	public void addGreenium(float alpha)
	{
		Vector3 tempChemical = new Vector3(redion, greenium, blurine);
		Vector3 chemicalDirection = Vector3.Project(tempChemical, new Vector3(0, 1, 0)) - tempChemical;
		tempChemical = tempChemical + chemicalDirection * alpha	;
		redion = tempChemical.x;
		greenium = tempChemical.y;
		blurine = tempChemical.z;
		this.normalizeColor();
	}
	
	public void addBlurine(float alpha)
	{
		Vector3 tempChemical = new Vector3(redion, greenium, blurine);
		Vector3 chemicalDirection = Vector3.Project(tempChemical,new Vector3(0, 0, 1)) - tempChemical;
		tempChemical = tempChemical + chemicalDirection * alpha	;
		redion = tempChemical.x;
		greenium = tempChemical.y;
		blurine = tempChemical.z;
		this.normalizeColor();
	}
	
	public void addChemical(float alpha, Color color)
	{
		Vector3 vecColor = new Vector3(color.r, color.g, color.b);
		Vector3 tempChemical = new Vector3(redion, greenium, blurine);
		Vector3 chemicalDirection = Vector3.Project(tempChemical, vecColor) - tempChemical;
		tempChemical = tempChemical + chemicalDirection * alpha	;
		redion = tempChemical.x;
		greenium = tempChemical.y;
		blurine = tempChemical.z;
		this.normalizeColor();
	}
	
	private void normalizeColor()
	{
		float magnitude = Mathf.Sqrt(redion * redion +
									greenium * greenium +
									blurine * blurine);
									
		this.redion /= magnitude;
		this.greenium /= magnitude;
		this.blurine /= magnitude;
	}
	
}
