using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class LuoiCauScript : MonoBehaviour
{
    public static LuoiCauScript instance;
	public float speed;
	public float speedMin;
	public float speedBegin;
	public Vector2 velocity;
	public float maxX;
	public float minX;
	public float minY;
	public float maxY;
	public Transform target;
	public bool cameraOut;
	public int type;
	public bool isUpSpeed;
	public float timeUpSpeed;
    public GameObject hook, halfHook;
	public float maxDistance;

    private Vector3 positionHalfHook, scaleHalfHook;
	private GameObject hookController;
	Vector3 prePosition;
	private float lineLength;

	// Use this for initialization
	void Start () {
        MakeInstance();
		isUpSpeed = false;
		prePosition = transform.localPosition;
		hookController = transform.parent.gameObject;

//		this.StartCoroutine("TimeUpSpeed");
	}
    void MakeInstance()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
	public IEnumerator TimeUpSpeed ()
	{
		while(true){
			if(isUpSpeed)
			{
				timeUpSpeed = timeUpSpeed - 1;
				if(timeUpSpeed <= 0)
					isUpSpeed = false;
			}
			yield return new WaitForSeconds (1);
		}
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("Toc do keo: " + speed);
		checkKeoCauXong();
        //Debug.Log("CameraOut " + cameraOut);
//		if(CGameManager.Instance.gameState == EnumStateGame.Play) 
		{
			//checkTouchScene();

			checkMoveOutCameraView();
		}
        if (CGameManager.instance.power || CGameManager.instance.powerCurrent)
        {
            speed = 4;
        }
        
        positionHalfHook = halfHook.gameObject.transform.position;
        //Debug.Log("Toa do cua cai luoi cau: x=" + positionHalfHook.x + " y=" + positionHalfHook.y);
        scaleHalfHook = halfHook.gameObject.transform.localScale;
		//勾到道具了
        if (hookController.GetComponent<DayCauScript>().typeAction == TypeAction.KeoCau)
        {
            GamePlayScript.instance.PlaySound(5);
            //Debug.Log("Dang keo");
            hook.SetActive(false);
            halfHook.SetActive(true);
            if (positionHalfHook.x > 0)
            {
                scaleHalfHook.x = -0.2f;
            }
            else
            {
                scaleHalfHook.x = 0.2f;
            }
            //halfHook.transform.localScale = scaleHalfHook;
        }
        else if (hookController.GetComponent<DayCauScript>().typeAction == TypeAction.Nghi)
        {
            hook.SetActive(true);
            halfHook.SetActive(false);
        }
    }
	void FixedUpdate() {
//		if(CGameManager.Instance.gameState == EnumStateGame.Play) 
		{
			//设置出钩速度
			if(hookController.GetComponent<DayCauScript>().typeAction == TypeAction.ThaCau)
            {
                GetComponent<Rigidbody2D>().velocity = velocity * speed;
            }
            else if(hookController.GetComponent<DayCauScript>().typeAction == TypeAction.KeoCau)
            {
				//到屏幕外收钩3倍速
                if (cameraOut)
                {
                    GetComponent<Rigidbody2D>().velocity = velocity * speed * 3;
                }
                else
                {
                    GetComponent<Rigidbody2D>().velocity = velocity * speed;
                }
            }
		}
	}

//	void OnTriggerEnter2D(Collider2D other) {
//		//		Debug.Log("enter");
//		if(other.gameObject.name.CompareTo("dau") == 0) {
//			GameObject fish = other.gameObject.transform.parent.gameObject;
//			fish.GetComponent<CFishScript>().fishAction = EnumFishAction.CanCau;
//			if(!isUpSpeed) {
//				if(speed > fish.GetComponent<CFishScript>().reduceSpeed) {
//					speed = speed - fish.GetComponent<CFishScript>().reduceSpeed;
//					if(speed < speedMin) 
//						speed = speedMin;
//				}
//			}
//
//			if(GameObject.Find("HookController").GetComponent<DayCauScript>().typeAction == TypeAction.ThaCau) {
//				GameObject.Find("HookController").GetComponent<DayCauScript>().typeAction = TypeAction.KeoCau;
//				velocity = -velocity;
//			}
//		}
//
//	}
	
	void OnTriggerExit2D(Collider2D other) {
//		Debug.Log("exit");
//		if(other.gameObject.name == "Hook") {
//			isBorder = false;
//		}
	}

	bool checkPositionOutBound() {
		return  gameObject.GetComponent<Renderer>().isVisible ;
	}

	public void checkTouchScene() { 	
		if(hookController.GetComponent<DayCauScript>().typeAction == TypeAction.Nghi)
		{
			hookController.GetComponent<DayCauScript>().typeAction = TypeAction.ThaCau;
			velocity = new Vector2(transform.position.x - target.position.x, 
			                       transform.position.y - target.position.y);
			velocity.Normalize();
			speed = speedBegin;
		}
	}
	//kiem tra khi luoi cau ra ngoai tam nhin cua camera
	void checkMoveOutCameraView() {
		if(hookController.GetComponent<DayCauScript>().typeAction == TypeAction.ThaCau) {
			lineLength = Vector2.Distance(hookController.transform.position, transform.position);
			//if (!checkPositionOutBound()) {
			//钩子到屏幕外了

			if (lineLength > maxDistance)
			{
				//改为出钩超过一定距离收回
				cameraOut = true;
				hookController.GetComponent<DayCauScript>().typeAction = TypeAction.KeoCau;
				velocity = -velocity;
			}
		}
	}

	//收钩完成
	void checkKeoCauXong() {
		if(transform.localPosition.y > maxY && hookController.GetComponent<DayCauScript>().typeAction == TypeAction.KeoCau) {
			//Debug.Log("keo cau xong");
			GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			hookController.GetComponent<DayCauScript>().typeAction = TypeAction.Nghi;
			transform.localPosition = prePosition;
		}
	}

    
}
