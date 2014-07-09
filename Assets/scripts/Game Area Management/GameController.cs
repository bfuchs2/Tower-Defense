using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject crate;
	public GameObject[] turrets;
	public int[] cost; //should be the same length as turrets
	public GameObject enemy;
	public float camSpeed;
	public float zoomSpeed;
	public GameObject spawnPoint;
	public int resources;
	public GUIText resourceText;
	public GUIText toastText;
	public TextMesh[] userInterface;

	//game boundaries
	private float xMin = -0.5f, zMax = 0.5f, zMin = -15.5f, xMax = 16.5f;

	void Start(){
		toastText.text = "";
		StartCoroutine(spawnWaves(50, 1f, 1.5f, 100f));
		AddToResources(0);
		for(int i = 0; i < userInterface.Length; i++){
			userInterface[i].text = "";
		}
		userInterface[0].transform.position = Vector3.one * 1000;
	}
	
	IEnumerator spawnWaves(int number, float wait, float speed, float health){
		for(int i = 0; i < number; i++){
			spawnEnemy(speed, health);
			yield return new WaitForSeconds(wait);
		}
	}

	private Vector3 camPosn, camMove, bottomLeft, topRight, mp;//mp created based on user's click and is used to determine where the menu should be and where to send new turrets
	float m;
	private Ray zero, one;
	private bool check = true;
	RaycastHit hit;
	GameObject selectedTurret;
	void Update (){
		camMove = new Vector3(Input.GetAxisRaw("Horizontal")*m, -Input.GetAxisRaw("Mouse ScrollWheel")*zoomSpeed, Input.GetAxisRaw("Vertical")*m) * Time.deltaTime;
		if(camMove.magnitude != 0 || check){//the following computation is only necesarry if the camera will move
			camPosn = Camera.main.transform.position + camMove;
			//camPosn = new Vector3(camPosn.x, camPosn.y, camPosn.z);//prevents deep copying
			//makes sure that the camera is within certain bounds such that the skybox is not visible
			zero = Camera.main.camera.ViewportPointToRay(new Vector3(0, 0, 0));
			one = Camera.main.ViewportPointToRay(new Vector3(1, 1, 0));
			//the boundaries of the camera at a given y, as two vector3s
			bottomLeft = Camera.main.transform.position.y*zero.direction/zero.direction.y + new Vector3(xMin, 0, zMin);
			topRight = Camera.main.transform.position.y*one.direction/one.direction.y + new Vector3(xMax, 0, zMax);
			Camera.main.transform.position = new Vector3(
				Mathf.Clamp(camPosn.x, bottomLeft.x, topRight.x),
				Mathf.Clamp(camPosn.y, 2, 13.8f),
				Mathf.Clamp(camPosn.z, bottomLeft.z, topRight.z)
			);
			m = camPosn.y * camSpeed;
			check = !check;
		}
		//if there is a click, send a turret to that location
		if(Input.GetMouseButtonDown(0)){
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 5)){
				UseMenu();
			}else if(Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 8)){
				DrawMenu();
			}else{
				foreach(TextMesh tm in userInterface) tm.text = "";
				userInterface[0].transform.position = Vector3.one * 1000; //to prevent the user from clicking invisible menus
			}
		}
		//makes sure there are enemies to shoot at
		if(GameObject.FindGameObjectWithTag("Enemy") == null) StartCoroutine(spawnWaves(10, 1, 1, 100));
	}


	private void UseMenu(){
		int type = (int) char.GetNumericValue(hit.collider.gameObject.GetComponent<TextMesh>().text.ToCharArray()[0]);
		if(userInterface[0].text == "Build Turret"){ //if we're building a turret
			type--;
			if(type < turrets.Length && type >= 0){
				if(AddToResources(-cost[type])){
					GameObject turretDrop = Instantiate(crate, mp + Vector3.up * 15, Quaternion.identity) as GameObject;
					turretDrop.GetComponent<CreateTurretFromCrate>().turret = turrets[type];
				}else{
					StartCoroutine(Toast("Insufficient Funds!"));
				}
			}
		}else{//we must be upgrading a turret
			if(type == 5){
				Recycle(selectedTurret);
			}else if(type > 0){
				StartCoroutine(Toast(selectedTurret.GetComponent<TurretAI>().Upgrade(type)));
			}
		}
		foreach(TextMesh tm in userInterface) tm.text = "";
		userInterface[0].transform.position = Vector3.one * 1000;
	}

	private void DrawMenu(){
		mp = new Vector3(round(hit.point.x), 0, round(hit.point.z-0.5f)-0.5f);//subtract by 0.5 because that way it works
		if(hit.collider.tag == "Landscape"){
			userInterface[0].transform.position = mp + new Vector3(0.5f, 0.15f, 0.5f);
			userInterface[0].text = "Build Turret";
			for(int i = 1; i < userInterface.Length; i++){
				userInterface[i].text = turrets.Length >= i ? i + ": " + turrets[i-1].ToString().Substring(0, turrets[i-1].ToString().IndexOf("(")) : "";
			}
		}else if(hit.collider.tag == "Turret"){
			userInterface[0].transform.position = mp + new Vector3(0.5f, 0.15f, 0.5f);
			int[] ups = hit.collider.GetComponent<TurretAI>().upgradeCosts;
			userInterface[0].text = "Upgrade " + hit.collider.ToString().Substring(0, hit.collider.ToString().IndexOf("("));
			userInterface[1].text = "1: range (" + ups[0] + ")";
			userInterface[2].text = "2: damage (" + ups[1] + ")";
			userInterface[3].text = "3: rotation speed (" + ups[2] + ")";
			userInterface[4].text = "4: fire rate (" + ups[3] + ")";
			userInterface[5].text = "5: recycle";
			selectedTurret = hit.collider.gameObject;
			/**range, damage, rotationSpeed, wait*/
		}
	}

	public void Recycle(GameObject gameObject){
		for(int i = 0; i < turrets.Length; i++){
			if(gameObject.ToString().Substring(0, 5) == turrets[i].ToString().Substring(0, 5)){
				AddToResources(cost[i]/2);
				Destroy(gameObject);
				return;
			}
		}
		Debug.Log("gameObject not found");
	}
	
	public bool AddToResources(int inc){
		if(resources + inc < 0){
			return false;
		}else{
			resources += inc;
			resourceText.text = "Resources: " + resources.ToString();
			return true;
		}
	}

	IEnumerator Toast(string s){
		toastText.text = s;
		yield return new WaitForSeconds(2f);
		toastText.text = "";
	}
	
	int round(float f){
		if(f % 1 >= 0.5) return (int) f + 1;
		return (int) f;
	}

	void spawnEnemy(float speed, float health){
		GameObject en = Instantiate(enemy, spawnPoint.transform.position + (Vector3.up* 0.02f), Quaternion.identity) as GameObject;
		EnemyBehaviourScript ebs = en.GetComponent<EnemyBehaviourScript>();
		ebs.health = health;
		ebs.setSpeed(speed);
	}
}