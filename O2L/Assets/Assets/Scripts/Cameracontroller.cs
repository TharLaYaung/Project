using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 繝励Ξ繧､繝､繝ｼ縺ｮ隕也せ繧定・辟ｶ縺ｫ霑ｽ蠕薙＆縺帙ｋ縺溘ａ縲√き繝｡繝ｩ縺ｮ謖吝虚繧貞宛蠕｡縺吶ｋ
public class Cameracontroller : MonoBehaviour
{
    private const float MIN_PITCH = -90f;
    private const float MAX_PITCH = 90f;
    private const string PREF_SENSITIVITY = "Sensitivity";
    private const string PREF_SMOOTH_SPEED = "SmoothSpeed";
    private const float DEFAULT_SENSITIVITY = 2f;
    private const float DEFAULT_SMOOTH_SPEED = 10f;
    private const float SHAKE_DURATION = 0.15f;
    private const float SHAKE_MAGNITUDE = 0.4f;

    [SerializeField] private GameObject player;
    [SerializeField] private Vector2 sensitivity;

    private float horizontalAngle;
    private Vector3 targetPosition;
    private Transform cameraTransform;

    [SerializeField] public float mouseSensitivity = 2f;
    public float speedV = 2.0f;
    public float speedH = 2.0f;

    private float xRotation = 0f;
    private float yRotation = 0f;
    private float currentXRotation = 0f;
    private float currentYRotation = 0f;

    public Camerashake camerashake;

    // 隕也阜螟悶・UI謫堺ｽ懊ｒ髦ｲ豁｢縺吶ｋ縺溘ａ縲√き繝ｼ繧ｽ繝ｫ繧偵Ο繝・け縺吶ｋ
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        this.targetPosition = this.player.transform.localPosition;
        
        currentXRotation = xRotation;
        currentYRotation = yRotation;
    }

    // 蜈･蜉帙↓蝓ｺ縺･縺阪き繝｡繝ｩ縺ｮ隗貞ｺｦ繧呈峩譁ｰ縺吶ｋ
    void Update()
    {
        // UI謫堺ｽ應ｸｭ縺ｫ繧ｫ繝｡繝ｩ縺悟虚縺九↑縺・ｈ縺・↓縺吶ｋ
        if (!PauseMenu.instance.isPaused)
        {
            float currentSensitivity = PlayerPrefs.GetFloat(PREF_SENSITIVITY, DEFAULT_SENSITIVITY);
            float smoothSpeed = PlayerPrefs.GetFloat(PREF_SMOOTH_SPEED, DEFAULT_SMOOTH_SPEED);

            xRotation -= currentSensitivity * Input.GetAxisRaw("Mouse Y");
            yRotation += currentSensitivity * Input.GetAxisRaw("Mouse X");

            // 隕也せ縺瑚｣剰ｿ斐▲縺ｦ謫堺ｽ應ｸ崎・縺ｫ縺ｪ繧九・繧帝亟縺舌◆繧√∽ｸ贋ｸ九・蝗櫁ｻ｢繧貞宛髯舌☆繧・
            xRotation = Mathf.Clamp(xRotation, MIN_PITCH, MAX_PITCH);

            currentXRotation = xRotation;
            currentYRotation = yRotation;

            transform.eulerAngles = new Vector3(currentXRotation, currentYRotation, 0f);
        }
    }

    // 繝励Ξ繧､繝､繝ｼ縺ｮ遘ｻ蜍募・逅・′螳御ｺ・＠縺溷ｾ後↓繧ｫ繝｡繝ｩ繧定ｿｽ蠕薙＆縺帙ｋ縺溘ａ縲´ateUpdate繧剃ｽｿ逕ｨ縺吶ｋ
    private void LateUpdate()
    {
        this.transform.localPosition += this.player.transform.localPosition - this.targetPosition;
        this.targetPosition = this.player.transform.localPosition;

        this.transform.RotateAround(this.player.transform.localPosition, Vector3.up, this.horizontalAngle);
    }

    // 陲ｫ蠑ｾ繧・匱遐ｲ譎ゅ・陦晄茶繧定ｦ冶ｦ夂噪縺ｫ莨昴∴繧九◆繧√√き繝｡繝ｩ繧呈昭繧峨☆
    public void CameraShake()
    {
        StartCoroutine(camerashake.Shake(SHAKE_DURATION, SHAKE_MAGNITUDE));
    }
}
