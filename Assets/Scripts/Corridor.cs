using UnityEngine;

public class Corridor
{
	public enum LoopDirection
	{
		Left,
		Right
	}

	public int startXPos;

	public int startYPos;

	public int corridorLength;

	public Direction direction;

	public int EndPositionX
	{
		get
		{
			if (direction == Direction.North || direction == Direction.South)
			{
				return startXPos;
			}
			if (direction == Direction.East)
			{
				return startXPos + corridorLength - 1;
			}
			return startXPos - corridorLength + 1;
		}
	}

	public int EndPositionY
	{
		get
		{
			if (direction == Direction.East || direction == Direction.West)
			{
				return startYPos;
			}
			if (direction == Direction.North)
			{
				return startYPos + corridorLength - 1;
			}
			return startYPos - corridorLength + 1;
		}
	}

	public void SetupCorridor(Room room, IntRange length, IntRange roomWidth, IntRange roomHeight, int columns, int rows, bool firstCorridor, float loopFactor, LoopDirection loopDirection)
	{
		this.direction = (Direction)Random.Range(0, 4);
		Direction direction = (Direction)((int)(room.enteringCorridor + 2) % 4);
		if (!firstCorridor && this.direction == direction)
		{
			int num = (int)this.direction;
			num++;
			num = (int)(this.direction = (Direction)(num % 4));
		}
		if (Random.value > loopFactor)
		{
			Direction direction2 = this.direction = (Direction)((int)(room.enteringCorridor + 3) % 4);
		}
		corridorLength = length.Random;
		int max = length.m_Max;
		switch (this.direction)
		{
		case Direction.North:
			startXPos = Random.Range(room.xPos, room.xPos + room.roomWidth - 1);
			startYPos = room.yPos + room.roomHeight;
			max = rows - startYPos - roomHeight.m_Min;
			break;
		case Direction.East:
			startXPos = room.xPos + room.roomWidth;
			startYPos = Random.Range(room.yPos, room.yPos + room.roomHeight - 1);
			max = columns - startXPos - roomWidth.m_Min;
			break;
		case Direction.South:
			startXPos = Random.Range(room.xPos, room.xPos + room.roomWidth);
			startYPos = room.yPos;
			max = startYPos - roomHeight.m_Min;
			break;
		case Direction.West:
			startXPos = room.xPos;
			startYPos = Random.Range(room.yPos, room.yPos + room.roomHeight);
			max = startXPos - roomWidth.m_Min;
			break;
		}
		corridorLength = Mathf.Clamp(corridorLength, 1, max);
	}
}
