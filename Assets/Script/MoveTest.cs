using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MoveTest : MonoBehaviour
{
	private int State;//��ɫ״̬
	private int oldState = 0;//ǰһ�ν�ɫ��״̬
	private int UP = 0;//��ɫ״̬��ǰ
	private int RIGHT = 1;//��ɫ״̬����
	private int DOWN = 2;//��ɫ״̬���
	private int LEFT = 3;//��ɫ״̬����
	public float speed = 8;

	void Start()
	{
	}
	void Update()
	{
		if (Input.GetKey("w"))
		{
			setState(UP);
		}
		else if (Input.GetKey("s"))
		{
			setState(DOWN);
		}

		if (Input.GetKey("a"))
		{
			setState(LEFT);
		}
		else if (Input.GetKey("d"))
		{
			setState(RIGHT);
		}

	}


	void setState(int currState)
	{
		Vector3 transformValue = new Vector3();//����ƽ������
		int rotateValue = (currState - State) * 90;
		transform.GetComponent<Animation>().Play("WalkForwardBattle");//���Ž�ɫ���߶���
		switch (currState)
		{
			case 0://��ɫ״̬��ǰʱ����ɫ������ǰ�����ƶ�
				transformValue = Vector3.forward * Time.deltaTime * speed;
				break;
			case 1://��ɫ״̬����ʱ����ɫ�������һ����ƶ�
				transformValue = Vector3.right * Time.deltaTime * speed;
				break;
			case 2://��ɫ״̬���ʱ����ɫ����������ƶ�
				transformValue = Vector3.back * Time.deltaTime * speed;
				break;
			case 3://��ɫ״̬����ʱ����ɫ�����������ƶ�
				transformValue = Vector3.left * Time.deltaTime * speed;
				break;
		}
		transform.Rotate(Vector3.up, rotateValue);//��ת��ɫ
		transform.Translate(transformValue, Space.World);//ƽ�ƽ�ɫ
		oldState = State;//��ֵ��������һ�μ���
		State = currState;//��ֵ��������һ�μ���
	}
}
