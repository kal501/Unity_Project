using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip audioClipTakeOutWeapon; //���� ���� ����

    private AudioSource audioSource; // ���� ��� ������Ʈ

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        // ���� ���� ���� ���
        PlaySound(audioClipTakeOutWeapon);
    }

    private void PlaySound(AudioClip clip)
    {
        audioSource.Stop(); // ������ ������� ���带 ������Ű��
        audioSource.clip = clip; // ���ο� ���� clip���� ��ü�� ��
        audioSource.Play(); // ���ο� ���带 �����Ų��.
    }
}
