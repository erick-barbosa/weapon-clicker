using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {
    private static Enemy instance_;

    public static Enemy Instance {
        get {
            if (instance_ == null) {
                GameObject obj = new GameObject("Enemy");
                instance_ = obj.AddComponent<Enemy>();
                DontDestroyOnLoad(obj);
            }
            return instance_;
        }
    }

    [SerializeField] private bool isImortal;
    public bool IsImortal { get; set; }

    private int health = 10;
    public int Health { get; private set; }

    [SerializeField] private GameObject damageTextPrefab;

    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Image healthBar;

    [SerializeField] private Image enemyImage;

    [SerializeField] private UIStateAnimator animator;

    [SerializeField] private List<AudioClip> hitSounds;
    [SerializeField] private AudioSource hitSource;

    private void Awake() {
        instance_ = this;

        animator = GetComponent<UIStateAnimator>();
        enemyImage = GetComponent<Image>();
    }

    private void Start() {
        ManageHealth();
    }

    private void ManageHealth() {
        if (isImortal)
            UpdateImortalHealth();
        else
            UpdateHealth();
    }

    public void TakeDamage(int damage) {
        var randomSound = hitSounds[Random.Range(0, hitSounds.Count)];

        Debug.Log("damage taken");

        if (health - damage > 0) {
            health -= damage;
        } else {
            health = 0;
        }

        animator.SetStateTemporary();
        ManageHealth();
        ShowDamageTaken(damage, Vector3.one);
        hitSource.PlayOneShot(randomSound);
    }

    public void UpdateHealth() {
        healthText.text = health.ToString();
        // change healthBar value
    }

    public void UpdateImortalHealth() {
        healthText.text = "";
        // change healthBar value
    }

    public void ShowDamageTaken(int damage, Vector3 position) {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        GameObject instance = Instantiate(damageTextPrefab, new Vector3(mousePos.x, mousePos.y, 1), Quaternion.identity, gameObject.transform);
        TextMeshProUGUI tmp = instance.GetComponent<TextMeshProUGUI>();

        tmp.text = damage.ToString();
    }
}
