using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
ケーブルの根っこのノードを「元ノード」
ケーブルのつなげる先のノードを「先ノード」と呼称 
先頭がiはInstantiateにより生成されたオブジェクト
*/

public class connector : MonoBehaviour
{
    [SerializeField]
    private GameObject cable;
    private Vector3 initialMousePos;
    private Vector3 mediumPos;

    private bool isCableInstantiated;
    private GameObject iCable;

    private Vector3 mousePos;

    private int lastCableId;

    private CableData iCableDataController;

    private NodeData nodeDataController;

    private bool isConnected;

    // Start is called before the first frame update
    void Start()
    {
        isCableInstantiated = false;
        lastCableId = 0;
        isConnected = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit)) {
                if (Input.GetMouseButton(0)) {
                    Debug.DrawRay(transform.position, ray.direction * hit.distance, Color.yellow);
                    mousePos = yOffset(hit.point, 0.5f);

                    if (hit.collider.tag == "Node" && !isCableInstantiated) {

                        isCableInstantiated = true;
                        initialMousePos = yOffset(hit.point, 0.5f);

                        // ケーブル生成
                        iCable = Instantiate(cable, hit.point, cable.transform.rotation).gameObject;

                        // ケーブルと元ノードのデータ制御を取得
                        iCableDataController = iCable.GetComponent<CableData>();
                        nodeDataController = hit.collider.GetComponent<NodeData>();
                    
                        // 生成したケーブルにケーブルIDを登録
                        iCableDataController.cableId = lastCableId++;

                        // 生成したケーブルに元ノードのノードIDを登録
                        iCableDataController.sourceId = nodeDataController.nodeId;

                        // ノードに生成したケーブルのケーブルIDを追加
                        nodeDataController.connectedCableIds.Add(iCableDataController.cableId);
                    }

                    if (isCableInstantiated && !isConnected) {
                        // 元ノードからマウスカーソルの方向に伸びるようにするスクリプト
                        
                        mediumPos = (mousePos - initialMousePos) / 2.0f; // オブジェクトの中心は２点間の中間
                        float dist = Vector3.Distance(mousePos, initialMousePos); // 距離を算出
                        iCable.transform.position = mediumPos + initialMousePos; // 元ノードのベクトルにオブジェクトの中心ベクトルを足す
                        iCable.transform.localScale = new Vector3(iCable.transform.localScale.x, iCable.transform.localScale.y, dist); // 元ノードからマウスカーソルまで伸ばす
                        Vector3 lookVec = new Vector3(mousePos.x, 0.5f, mousePos.z); // 以下２行はマウスカーソルの方向に向ける
                        iCable.transform.LookAt(lookVec);
                    }
                } else if (Input.GetMouseButtonUp(0)) {
                    if(hit.collider.tag == "Node" && isCableInstantiated) {
                        // 先ノードのデータ制御を取得
                        nodeDataController = hit.collider.GetComponent<NodeData>();

                        // 接続先ノードは元ノードでなければ接続する。
                        // 元ノードであるときは接続しないでケーブルを消す
                        if (nodeDataController.nodeId != iCableDataController.sourceId) {
                            isConnected = true;

                            // ケーブルに先ノードIDを登録
                            iCableDataController.targetId = nodeDataController.nodeId;

                            // 先ノードにケーブルIDを登録
                            nodeDataController.connectedCableIds.Add(iCableDataController.cableId);

                            // 終了処理的な？
                            finish_process();

                        } else {
                            Debug.Log("nodeId error");
                            Destroy(iCable.gameObject);
                            finish_process();
                        }
                    } else {
                        Debug.Log("isCableInstantiated is false");
                        Destroy(iCable.gameObject);
                        finish_process();
                    }

                }
            }
        }
    }

    private Vector3 yOffset(Vector3 vec, float offset) {
        return new Vector3(vec.x, offset, vec.z);
    }

    private void finish_process() {
        isCableInstantiated = false;
        iCable = null;
        nodeDataController = null;
        iCableDataController = null;
        isConnected = false;
    }
}
