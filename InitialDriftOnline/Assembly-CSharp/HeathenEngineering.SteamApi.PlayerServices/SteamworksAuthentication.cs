using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HeathenEngineering.SteamApi.Foundation;
using Steamworks;
using UnityEngine;

namespace HeathenEngineering.SteamApi.PlayerServices;

public static class SteamworksAuthentication
{
	[Serializable]
	public class Session
	{
		public bool isClientSession = true;

		public CSteamID User;

		public CSteamID GameOwner;

		public byte[] Data;

		public EAuthSessionResponse Responce;

		public Action<Session> OnStartCallback;

		public bool IsBarrowed => User != GameOwner;

		public void End()
		{
			if (isClientSession)
			{
				SteamUser.EndAuthSession(User);
			}
			else
			{
				SteamGameServer.EndAuthSession(User);
			}
		}
	}

	[Serializable]
	public class Ticket
	{
		public bool isClientTicket = true;

		public HAuthTicket Handle;

		public byte[] Data;

		public bool Verified;

		public uint CreatedOn;

		public TimeSpan Age => new TimeSpan(0, 0, (int)(SteamUtils.GetServerRealTime() - CreatedOn));

		public void Cancel()
		{
			if (isClientTicket)
			{
				SteamUser.CancelAuthTicket(Handle);
			}
			else
			{
				SteamGameServer.CancelAuthTicket(Handle);
			}
		}
	}

	public static List<Ticket> ActiveTickets;

	public static List<Session> ActiveSessions;

	private static Callback<GetAuthSessionTicketResponse_t> m_GetAuthSessionTicketResponce;

	private static Callback<GetAuthSessionTicketResponse_t> m_GetAuthSessionTicketResponceServer;

	private static Callback<ValidateAuthTicketResponse_t> m_ValidateAuthSessionTicketResponce;

	private static Callback<ValidateAuthTicketResponse_t> m_ValidateAuthSessionTicketResponceServer;

	private static bool callbacksRegistered;

	public static bool RegisterCallbacks()
	{
		if (SteamSettings.current.Initialized)
		{
			if (!callbacksRegistered)
			{
				callbacksRegistered = true;
				m_GetAuthSessionTicketResponce = Callback<GetAuthSessionTicketResponse_t>.Create(HandleGetAuthSessionTicketResponce);
				m_GetAuthSessionTicketResponceServer = Callback<GetAuthSessionTicketResponse_t>.CreateGameServer(HandleGetAuthSessionTicketResponce);
				m_ValidateAuthSessionTicketResponce = Callback<ValidateAuthTicketResponse_t>.Create(HandleValidateAuthTicketResponse);
				m_ValidateAuthSessionTicketResponceServer = Callback<ValidateAuthTicketResponse_t>.CreateGameServer(HandleValidateAuthTicketResponse);
				return true;
			}
			return true;
		}
		return false;
	}

	public static bool IsAuthTicketValid(Ticket ticket)
	{
		RegisterCallbacks();
		if (ticket.Handle == default(HAuthTicket) || ticket.Handle == HAuthTicket.Invalid)
		{
			return false;
		}
		return true;
	}

	public static string EncodedAuthTicket(Ticket ticket)
	{
		RegisterCallbacks();
		if (!IsAuthTicketValid(ticket))
		{
			return "";
		}
		StringBuilder stringBuilder = new StringBuilder();
		byte[] data = ticket.Data;
		foreach (byte b in data)
		{
			stringBuilder.AppendFormat("{0:X2}", b);
		}
		return stringBuilder.ToString();
	}

	public static Ticket ClientGetAuthSessionTicket()
	{
		RegisterCallbacks();
		Ticket ticket = new Ticket();
		ticket.isClientTicket = true;
		ticket.Data = new byte[1024];
		ticket.Handle = SteamUser.GetAuthSessionTicket(ticket.Data, 1024, out var pcbTicket);
		ticket.CreatedOn = SteamUtils.GetServerRealTime();
		Array.Resize(ref ticket.Data, (int)pcbTicket);
		if (ActiveTickets == null)
		{
			ActiveTickets = new List<Ticket>();
		}
		ActiveTickets.Add(ticket);
		return ticket;
	}

	public static Ticket ServerGetAuthSessionTicket()
	{
		RegisterCallbacks();
		Ticket ticket = new Ticket();
		ticket.isClientTicket = false;
		ticket.Data = new byte[1024];
		ticket.Handle = SteamGameServer.GetAuthSessionTicket(ticket.Data, 1024, out var pcbTicket);
		ticket.CreatedOn = SteamUtils.GetServerRealTime();
		Array.Resize(ref ticket.Data, (int)pcbTicket);
		if (ActiveTickets == null)
		{
			ActiveTickets = new List<Ticket>();
		}
		ActiveTickets.Add(ticket);
		return ticket;
	}

	public static void CancelAuthTicket(Ticket ticket)
	{
		RegisterCallbacks();
		ticket.Cancel();
	}

	public static void ClientBeginAuthSession(byte[] authTicket, CSteamID user, Action<Session> callback)
	{
		RegisterCallbacks();
		Session item = new Session
		{
			isClientSession = true,
			User = user,
			OnStartCallback = callback
		};
		if (ActiveSessions == null)
		{
			ActiveSessions = new List<Session>();
		}
		ActiveSessions.Add(item);
		SteamUser.BeginAuthSession(authTicket, authTicket.Length, user);
	}

	public static void ServerBeginAuthSession(byte[] authTicket, CSteamID user, Action<Session> callback)
	{
		RegisterCallbacks();
		Session item = new Session
		{
			isClientSession = false,
			User = user,
			OnStartCallback = callback
		};
		if (ActiveSessions == null)
		{
			ActiveSessions = new List<Session>();
		}
		ActiveSessions.Add(item);
		SteamGameServer.BeginAuthSession(authTicket, authTicket.Length, user);
	}

	public static void ClientEndAuthSession(CSteamID user)
	{
		SteamUser.EndAuthSession(user);
	}

	public static void ServerEndAuthSession(CSteamID user)
	{
		SteamGameServer.EndAuthSession(user);
	}

	private static void HandleGetAuthSessionTicketResponce(GetAuthSessionTicketResponse_t pCallback)
	{
		if (ActiveTickets != null && ActiveTickets.Any((Ticket p) => p.Handle == pCallback.m_hAuthTicket))
		{
			Ticket ticket = ActiveTickets.First((Ticket p) => p.Handle == pCallback.m_hAuthTicket);
			if (ticket.Handle != default(HAuthTicket) && ticket.Handle != HAuthTicket.Invalid && pCallback.m_eResult == EResult.k_EResultOK)
			{
				ticket.Verified = true;
			}
		}
	}

	private static void HandleValidateAuthTicketResponse(ValidateAuthTicketResponse_t param)
	{
		if (ActiveSessions != null && ActiveSessions.Any((Session p) => p.User == param.m_SteamID))
		{
			Session session = ActiveSessions.First((Session p) => p.User == param.m_SteamID);
			session.Responce = param.m_eAuthSessionResponse;
			session.GameOwner = param.m_OwnerSteamID;
			Debug.Log("Processing session request data for " + param.m_SteamID.m_SteamID.ToString() + " status = " + param.m_eAuthSessionResponse);
			if (session.OnStartCallback != null)
			{
				session.OnStartCallback(session);
			}
		}
		else
		{
			Debug.LogWarning("Recieved an authentication ticket responce for user " + param.m_SteamID.m_SteamID + " no matching session was found for this user.");
		}
	}

	public static void EndAllSessions()
	{
		foreach (Session activeSession in ActiveSessions)
		{
			activeSession.End();
		}
	}

	public static void CancelAllTickets()
	{
		foreach (Ticket activeTicket in ActiveTickets)
		{
			activeTicket.Cancel();
		}
	}
}
