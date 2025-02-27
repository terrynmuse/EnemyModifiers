﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace FargoEnemyModifiers.Modifiers
{
	public class WormBody : Modifier
	{
		public WormBody()
		{
			name = "Worm";
			kbMultiplier = 0;
		}

		private bool firstTick = true;

		public override bool PreAI(NPC npc)
		{
			if (firstTick)
			{
				UpdateModifierStats(npc);
			}

			firstTick = false;

			WormAI(npc);

			return false;
		}

		private void WormAI(NPC npc)
		{
			bool flag = false;
			float num4 = 0.2f;

			if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || (flag && (double)Main.player[npc.target].position.Y < Main.worldSurface * 16.0))
			{
				npc.TargetClosest(true);
			}
			if (Main.player[npc.target].dead || (flag && (double)Main.player[npc.target].position.Y < Main.worldSurface * 16.0))
			{
				if (npc.timeLeft > 300)
				{
					npc.timeLeft = 300;
				}
				if (flag)
				{
					npc.velocity.Y = npc.velocity.Y + num4;
				}
			}
			if (Main.netMode != 1)
			{
				if (!Main.npc[npc.realLife].active)
				{
					npc.life = 0;
					npc.HitEffect(0, 10.0);
					npc.active = false;
					NetMessage.SendData(28, -1, -1, null, npc.whoAmI, -1f, 0f, 0f, 0, 0, 0);
				}

                

                if (!npc.active && Main.netMode == 2)
				{
					NetMessage.SendData(28, -1, -1, null, npc.whoAmI, -1f, 0f, 0f, 0, 0, 0);
				}
			}
			int num29 = (int)(npc.position.X / 16f) - 1;
			int num30 = (int)((npc.position.X + (float)npc.width) / 16f) + 2;
			int num31 = (int)(npc.position.Y / 16f) - 1;
			int num32 = (int)((npc.position.Y + (float)npc.height) / 16f) + 2;
			if (num29 < 0)
			{
				num29 = 0;
			}
			if (num30 > Main.maxTilesX)
			{
				num30 = Main.maxTilesX;
			}
			if (num31 < 0)
			{
				num31 = 0;
			}
			if (num32 > Main.maxTilesY)
			{
				num32 = Main.maxTilesY;
			}
			bool flag2 = false;

			if (!flag2)
			{
				for (int num33 = num29; num33 < num30; num33++)
				{
					for (int num34 = num31; num34 < num32; num34++)
					{
						if (Main.tile[num33, num34] != null && ((Main.tile[num33, num34].nactive() && (Main.tileSolid[(int)Main.tile[num33, num34].type] || (Main.tileSolidTop[(int)Main.tile[num33, num34].type] && Main.tile[num33, num34].frameY == 0))) || Main.tile[num33, num34].liquid > 64))
						{
							Vector2 vector;
							vector.X = (float)(num33 * 16);
							vector.Y = (float)(num34 * 16);
							if (npc.position.X + (float)npc.width > vector.X && npc.position.X < vector.X + 16f && npc.position.Y + (float)npc.height > vector.Y && npc.position.Y < vector.Y + 16f)
							{
								flag2 = true;
								if (Main.rand.Next(100) == 0 && Main.tile[num33, num34].nactive())
								{
									WorldGen.KillTile(num33, num34, true, true, false);
								}
							}
						}
					}
				}
			}

			float num37 = 8f;
			float num38 = 0.07f;
			Vector2 vector2 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
			float num40 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2);
			float num41 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2);

			num40 = (float)((int)(num40 / 16f) * 16);
			num41 = (float)((int)(num41 / 16f) * 16);
			vector2.X = (float)((int)(vector2.X / 16f) * 16);
			vector2.Y = (float)((int)(vector2.Y / 16f) * 16);
			num40 -= vector2.X;
			num41 -= vector2.Y;

			float num53 = (float)Math.Sqrt((double)(num40 * num40 + num41 * num41));
			if (npc.localAI[1] > 0f && npc.localAI[1] < (float)Main.npc.Length)
			{
				try
				{
					vector2 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
					num40 = Main.npc[(int)npc.localAI[1]].position.X + (float)(Main.npc[(int)npc.localAI[1]].width / 2) - vector2.X;
					num41 = Main.npc[(int)npc.localAI[1]].position.Y + (float)(Main.npc[(int)npc.localAI[1]].height / 2) - vector2.Y;
				}
				catch
				{
				}
				npc.rotation = (float)Math.Atan2((double)num41, (double)num40) + 1.57f;
				num53 = (float)Math.Sqrt((double)(num40 * num40 + num41 * num41));
				int num54 = npc.width;

				num53 = (num53 - (float)num54) / num53;
				num40 *= num53;
				num41 *= num53;
				npc.velocity = Vector2.Zero;
				npc.position.X = npc.position.X + num40;
				npc.position.Y = npc.position.Y + num41;


			}
			else
			{
				if (!flag2)
				{
					npc.TargetClosest(true);
					npc.velocity.Y = npc.velocity.Y + 0.11f;
					if (npc.velocity.Y > num37)
					{
						npc.velocity.Y = num37;
					}
					if ((double)(Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y)) < (double)num37 * 0.4)
					{
						if (npc.velocity.X < 0f)
						{
							npc.velocity.X = npc.velocity.X - num38 * 1.1f;
						}
						else
						{
							npc.velocity.X = npc.velocity.X + num38 * 1.1f;
						}
					}
					else if (npc.velocity.Y == num37)
					{
						if (npc.velocity.X < num40)
						{
							npc.velocity.X = npc.velocity.X + num38;
						}
						else if (npc.velocity.X > num40)
						{
							npc.velocity.X = npc.velocity.X - num38;
						}
					}
					else if (npc.velocity.Y > 4f)
					{
						if (npc.velocity.X < 0f)
						{
							npc.velocity.X = npc.velocity.X + num38 * 0.9f;
						}
						else
						{
							npc.velocity.X = npc.velocity.X - num38 * 0.9f;
						}
					}
				}
				else
				{
					if (npc.soundDelay == 0)
					{
						float num55 = num53 / 40f;
						if (num55 < 10f)
						{
							num55 = 10f;
						}
						if (num55 > 20f)
						{
							num55 = 20f;
						}
						npc.soundDelay = (int)num55;
						Main.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 1, 1f, 0f);
					}
					num53 = (float)Math.Sqrt((double)(num40 * num40 + num41 * num41));
					float num56 = Math.Abs(num40);
					float num57 = Math.Abs(num41);
					float num58 = num37 / num53;
					num40 *= num58;
					num41 *= num58;
					bool flag4 = false;


					if (flag4)
					{
						bool flag5 = true;
						for (int num59 = 0; num59 < 255; num59++)
						{
							if (Main.player[num59].active && !Main.player[num59].dead && Main.player[num59].ZoneCorrupt)
							{
								flag5 = false;
							}
						}
						if (flag5)
						{
							if (Main.netMode != 1 && (double)(npc.position.Y / 16f) > (Main.rockLayer + (double)Main.maxTilesY) / 2.0)
							{
								npc.active = false;
								int num60 = (int)npc.localAI[0];
								while (num60 > 0 && num60 < 200 && Main.npc[num60].active && Main.npc[num60].aiStyle == npc.aiStyle)
								{
									int num61 = (int)Main.npc[num60].localAI[0];
									Main.npc[num60].active = false;
									npc.life = 0;
									if (Main.netMode == 2)
									{
										NetMessage.SendData(23, -1, -1, null, num60, 0f, 0f, 0f, 0, 0, 0);
									}
									num60 = num61;
								}
								if (Main.netMode == 2)
								{
									NetMessage.SendData(23, -1, -1, null, npc.whoAmI, 0f, 0f, 0f, 0, 0, 0);
								}
							}
							num40 = 0f;
							num41 = num37;
						}
					}
					bool flag6 = false;


					if (!flag6)
					{
						if ((npc.velocity.X > 0f && num40 > 0f) || (npc.velocity.X < 0f && num40 < 0f) || (npc.velocity.Y > 0f && num41 > 0f) || (npc.velocity.Y < 0f && num41 < 0f))
						{
							if (npc.velocity.X < num40)
							{
								npc.velocity.X = npc.velocity.X + num38;
							}
							else if (npc.velocity.X > num40)
							{
								npc.velocity.X = npc.velocity.X - num38;
							}
							if (npc.velocity.Y < num41)
							{
								npc.velocity.Y = npc.velocity.Y + num38;
							}
							else if (npc.velocity.Y > num41)
							{
								npc.velocity.Y = npc.velocity.Y - num38;
							}
							if ((double)Math.Abs(num41) < (double)num37 * 0.2 && ((npc.velocity.X > 0f && num40 < 0f) || (npc.velocity.X < 0f && num40 > 0f)))
							{
								if (npc.velocity.Y > 0f)
								{
									npc.velocity.Y = npc.velocity.Y + num38 * 2f;
								}
								else
								{
									npc.velocity.Y = npc.velocity.Y - num38 * 2f;
								}
							}
							if ((double)Math.Abs(num40) < (double)num37 * 0.2 && ((npc.velocity.Y > 0f && num41 < 0f) || (npc.velocity.Y < 0f && num41 > 0f)))
							{
								if (npc.velocity.X > 0f)
								{
									npc.velocity.X = npc.velocity.X + num38 * 2f;
								}
								else
								{
									npc.velocity.X = npc.velocity.X - num38 * 2f;
								}
							}
						}
						else if (num56 > num57)
						{
							if (npc.velocity.X < num40)
							{
								npc.velocity.X = npc.velocity.X + num38 * 1.1f;
							}
							else if (npc.velocity.X > num40)
							{
								npc.velocity.X = npc.velocity.X - num38 * 1.1f;
							}
							if ((double)(Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y)) < (double)num37 * 0.5)
							{
								if (npc.velocity.Y > 0f)
								{
									npc.velocity.Y = npc.velocity.Y + num38;
								}
								else
								{
									npc.velocity.Y = npc.velocity.Y - num38;
								}
							}
						}
						else
						{
							if (npc.velocity.Y < num41)
							{
								npc.velocity.Y = npc.velocity.Y + num38 * 1.1f;
							}
							else if (npc.velocity.Y > num41)
							{
								npc.velocity.Y = npc.velocity.Y - num38 * 1.1f;
							}
							if ((double)(Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y)) < (double)num37 * 0.5)
							{
								if (npc.velocity.X > 0f)
								{
									npc.velocity.X = npc.velocity.X + num38;
								}
								else
								{
									npc.velocity.X = npc.velocity.X - num38;
								}
							}
						}
					}
				}
				npc.rotation = (float)Math.Atan2((double)npc.velocity.Y, (double)npc.velocity.X) + 1.57f;
			}
		}
	}
}
