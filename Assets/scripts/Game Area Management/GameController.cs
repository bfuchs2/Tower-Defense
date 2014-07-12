using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject crate;
	public RangeAnimatorScript rangeAnimatorScript;
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
	bool gameOver = false;
	public GUIText switchback;

	Wave[] waves = {
			new Wave(1, 1, 100, 13),
			new Wave(0.7f, 1, 100, 13),
			new Wave(1, 1.3f, 100, 16),
			new Wave(1, 2, 50, 10),
			new Wave(3.5f, 3.3f, 75, 6),
			new Wave(0.7f, 1.1f, 250, 6),
			new Wave(0.3f, 5, 10, 103),
			new Wave(3, 0.2f, 2000, 13)
	};
	int place;
	void Awake(){
		place = 0;
	}

	void Start(){
		rangeAnimatorScript = GameObject.FindGameObjectWithTag("Range").GetComponent<RangeAnimatorScript>();
		toastText.text = "";
		switchback.text = "";
		AddToResources(0);
		for(int i = 0; i < userInterface.Length; i++){
			userInterface[i].text = "";
		}
		userInterface[0].transform.position = Vector3.one * 1000;
	}
	
	void SpawnNextWave(){
		Wave w = waves[place];
		StartCoroutine(SpawnWaves(w.number, w.wait, w.speed, w.health));
		place++;
	}

	IEnumerator SpawnWaves(int number, float wait, float speed, float health){
		for(int i = 0; i < number; i++){
			spawnEnemy(speed, health);
			yield return new WaitForSeconds(wait);
		}
	}

	private Vector3 mp;//mp created based on user's click and is used to determine where the menu should be and where to send new turrets
	private Ray zero, one;
	RaycastHit hit;
	GameObject selectedTurret;
	bool cameraInTurretView;
	void Update (){
		//if there is a click, send a turret to that location
		if(cameraInTurretView){
			if(Input.GetKey(KeyCode.S)){
				Camera.main.GetComponent<CameraManager>().SwitchToTurretView(null);
				cameraInTurretView = false;
				switchback.text = "";
			}
		}else{
			if(Input.GetMouseButtonDown(0)){
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if(Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 5)){//UI layer
					UseMenu();
				}else if(Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 8)){ //default layer
					DrawMenu();
				}else{
					foreach(TextMesh tm in userInterface) tm.text = "";
					userInterface[0].transform.position = Vector3.one * 1000; //to prevent the user from clicking invisible menus
				}
			}
		}
		//makes sure there are enemies to shoot at
		if(GameObject.FindGameObjectWithTag("Enemy") == null && !gameOver){
			if(place == waves.Length){
				gameOver = true;
				toastText.text = "You Win!";
			}else{
				SpawnNextWave();
			}
		}
	}

	public void EndGame(){
		gameOver = true;
		toastText.text = "Game Ogre!";
	}

	private void UseMenu(){
		bool erase = true;
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
			}else if(type == 6){
				Camera.main.GetComponent<CameraManager>().SwitchToTurretView(selectedTurret);
				cameraInTurretView = true;
				rangeAnimatorScript.SetPositionAndReset(selectedTurret.transform.position, selectedTurret.GetComponent<TurretAI>().att[0]);
				switchback.text = "press 's' to switch back";
			}else if(type > 0){
				StartCoroutine(Toast(selectedTurret.GetComponent<TurretAI>().Upgrade(type)));
				if(toastText.text == ""){
					DrawMenu();
					erase = false;
				}
			}
		}
		if(erase){
			foreach(TextMesh tm in userInterface) tm.text = "";
			userInterface[0].transform.position = Vector3.one * 1000;
			if(switchback.text.Length == 0) rangeAnimatorScript.Disable();
		}
	}

	private void DrawMenu(){
		mp = new Vector3(round(hit.point.x), 0, round(hit.point.z-0.5f)-0.5f);//subtract by 0.5 because that way it works
		if(hit.collider.tag == "Landscape"){
			userInterface[0].transform.position = mp + new Vector3(0.5f, 0.15f, 0.5f);
			userInterface[0].text = "Build Turret";
			for(int i = 1; i < userInterface.Length; i++){
				userInterface[i].text = turrets.Length >= i ? i + ": " + turrets[i-1].ToString().Substring(0, turrets[i-1].ToString().IndexOf("("))  + "(" + cost[i-1] + ")" : "";
			}
			rangeAnimatorScript.Disable();
			return;
		}
		if(hit.collider.tag == "Turret"){
			selectedTurret = hit.collider.gameObject;
			userInterface[0].transform.position = mp + new Vector3(0.5f, 0.15f, 0.5f);
			userInterface[0].text = "Upgrade " + hit.collider.ToString().Substring(0, hit.collider.ToString().IndexOf("("));
		}
		if(selectedTurret != null){
			rangeAnimatorScript.SetPositionAndReset(selectedTurret.transform.position, selectedTurret.GetComponent<TurretAI>().att[0]);
			int[] ups = selectedTurret.GetComponent<TurretAI>().upgradeCosts;
			userInterface[1].text = "1: range (" + ups[0] + ")";
			userInterface[2].text = "2: damage (" + ups[1] + ")";
			userInterface[3].text = "3: rotation speed (" + ups[2] + ")";
			userInterface[4].text = "4: fire rate (" + ups[3] + ")";
			userInterface[5].text = "5: recycle";
			userInterface[6].text = "6: switch to view";
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