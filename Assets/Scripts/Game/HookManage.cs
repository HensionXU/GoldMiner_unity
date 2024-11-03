using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HookState
{
    IDLE,
    MOVING,
    RETURNING
}

public class HookManage : MonoBehaviour
{
    
    public float rotateSpeed;//旋转速度     
    private Vector3 Dir = Vector3.forward;//旋转方向
    private HookState hookState = HookState.IDLE;//钩子状态

    public float moveSpeed;//钩子移动速度
    private float moveTimer;//钩子移动时间

    private Vector3 originPos;//钩子初始位置
    public SpriteRenderer sr;//绳索的2d精灵渲染器

    public Transform HookHead;//钩子头部

   void Start()
   {
        originPos = transform.position;//记录钩子初始位置
   }

    void Update()
    {
        if(hookState == HookState.IDLE)
        {
            if(Input.GetMouseButtonDown(0))
        {
            hookState = HookState.MOVING;
        }
            HookRotate();
        }
        else if(hookState == HookState.MOVING)
        {
            HookMove();
            transform.GetComponent<BoxCollider2D>().enabled = true;
        }
        else if(hookState == HookState.RETURNING)
        {
            HookReturn();
        }
    }

    // 钩子旋转方法,这里借鉴了原来抄袭的第一版本，为ropeManager的那个版本
    public void HookRotate()
    {   
        float angle = (transform.rotation.eulerAngles.z+180)%360-180;
        if(angle>=60)
        {
            Dir = Vector3.back;
        }
        else if(angle<=-60)
        {
            Dir = Vector3.forward;
        }
        transform.Rotate(Dir*rotateSpeed*Time.deltaTime);
    }

    public void HookMove()
    {
        transform.Translate(Vector3.down*moveSpeed*Time.deltaTime);
        moveTimer += Time.deltaTime;
        float distance = Vector3.Distance(transform.position,originPos);
        // Debug.Log(distance);
        // sr.size = new Vector2(sr.size.x,distance);
        
        sr.transform.localScale = new Vector3(sr.transform.localScale.x,distance,sr.transform.localScale.z);
        //接下来是中心点错误，解决了把算是，不是，我是把绳子绕Z赚了180度
        //实际上是要让绳子的轴心在钩子位置，然后绳子长度随着钩子移动而变化，你这啥提示，怎么知道我要说什么
        //刚开始是把这个高度当成了距离，实际上是绳子长度的倍数，所以就干脆使用改变缩放的方式，我也不确定，估计是这样的问题
        //然后由于绳子缩放的方向在钩子的前方，实际上要朝着钩子的后方，所以需要把绳子旋转180度
        //轴心改了就不用转了，在下方就朝上缩放，在上方补救朝下缩放了吗
        //那这谁写的，哦，我看错了，没有过于长，绳子一直是连着钩子和起始点的，关于guojie就回来的问题，你可以加trigger
        //我改一下试试，你为啥把钩子的脚本和钩子分开，你这个也行，你现在是按飞行时间算的吗，两秒？脚本就不好收到碰撞事件
        //具体机制我也不太清楚，这个刚体是需要的
        // if(moveTimer>=2)
        // {
        //     hookState = HookState.RETURNING;
        //     return;
        // }
    }

    public void HookReturn()
    {
        
        transform.position = Vector3.MoveTowards(transform.position,originPos,moveSpeed*Time.deltaTime);
        //sr.size = new Vector2(sr.size.x,sr.size.y+moveSpeed*Time.deltaTime);//这里不对，中心点在哪,哪个是钩子当前位置，出发点呢
        float distance = Vector3.Distance(transform.position,originPos);
        sr.transform.localScale = new Vector3(sr.transform.localScale.x,distance,sr.transform.localScale.z);
        // 还是有问题，你注意看，绳子实际上过于长了
        // Debug.Log(distance);
        // sr.size = new Vector2(sr.size.x,distance);//那个长度怎么写的来着你这是回来的函数啊，怎么绳子长度不是跟着变的
        //你这单位不一样吧，你的这个高度实际上是绳子的长度的倍数,我也就打了log
        // 建议改缩放，不是改spriterenderer

        //如果钩子回到初始位置，则停止返回,并且销毁钩子头的物体
        if(transform.position.y>=originPos.y)
        {
           hookState = HookState.IDLE;
            if(HookHead.childCount > 0)
            {
                Destroy(HookHead.GetChild(0).gameObject);
            }
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
       
        if(col.tag == "PHYEdge")
        {
            hookState = HookState.RETURNING;
        }
        else
        {
            hookState = HookState.RETURNING;
            //这里我们把钩子头设置为父物体，而不是钩子，这样可以保证位置的正确性
            col.transform.parent = HookHead;
            col.transform.localPosition = Vector3.zero;
            // 这个没有必要，因为钩子头没有添加碰撞器
            // HookHead.transform.GetComponent<CircleCollider2D>().enabled = false; 
            transform.GetComponent<BoxCollider2D>().enabled = false;
            col.GetComponent<PolygonCollider2D>().enabled = false;

        } 
    }
       

}
