using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//注意，如果需要获取UI组件，需要添加这个命名空间，否则会报错
using UnityEngine.UI;

public class Game_load : MonoBehaviour
{
    public Slider ProgressBar;
    void Awake()
    {
        StartCoroutine(LoadStartScene());
    }
   
    // 这是一个异步加载的协程，用来加载游戏的开始页面
    // 同时在里面添加了进度条，用来显示加载的进度
   IEnumerator LoadStartScene()
   {
        yield return new WaitForSeconds(1f);
        AsyncOperation Operation = SceneManager.LoadSceneAsync("Start");
        while (!Operation.isDone)
        {
            float progress = Operation.progress / 0.9f;
            ProgressBar.value = progress;
            yield return null;
        }
   }
}
