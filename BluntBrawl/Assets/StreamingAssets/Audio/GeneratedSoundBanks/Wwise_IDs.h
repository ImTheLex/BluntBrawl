/////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Audiokinetic Wwise generated include file. Do not edit.
//
/////////////////////////////////////////////////////////////////////////////////////////////////////

#ifndef __WWISE_IDS_H__
#define __WWISE_IDS_H__

#include <AK/SoundEngine/Common/AkTypes.h>

namespace AK
{
    namespace EVENTS
    {
        static const AkUniqueID AMBIENTTEST = 1500071851U;
        static const AkUniqueID BACKGROUNDSEVENTSSOUNDS = 1816771275U;
        static const AkUniqueID BUTTONS = 203100604U;
        static const AkUniqueID ENEMYEVENTSDASH = 3175741232U;
        static const AkUniqueID ENEMYEVENTSDIE = 723260870U;
        static const AkUniqueID ENEMYEVENTSFALL = 2172152223U;
        static const AkUniqueID ENEMYEVENTSFOOTSTEPS = 2772879681U;
        static const AkUniqueID ENEMYEVENTSSLASH = 3292113943U;
        static const AkUniqueID ENEMYWEAPONSEVENTHITS = 526621624U;
        static const AkUniqueID ENEMYWEAPONSEVENTSDROP = 1389296758U;
        static const AkUniqueID MUSIC = 3991942870U;
        static const AkUniqueID OBJECTEVENTSCATHAMMERONGROUND = 2963827865U;
        static const AkUniqueID OBJECTEVENTSCRATEHIT = 2354176855U;
        static const AkUniqueID OBJECTEVENTSFENCEHIT = 4263813725U;
        static const AkUniqueID OBJECTEVENTSHEAL = 2381628217U;
        static const AkUniqueID PLAYEREVENTSDASH = 1505439485U;
        static const AkUniqueID PLAYEREVENTSDIE = 1082178505U;
        static const AkUniqueID PLAYEREVENTSFALL = 2509028246U;
        static const AkUniqueID PLAYEREVENTSFOOTSTEPS = 1744576738U;
        static const AkUniqueID PLAYEREVENTSSLASH = 2607198388U;
        static const AkUniqueID PLAYERWEAPONSEVENTHITS = 4152903161U;
        static const AkUniqueID PLAYERWEAPONSEVENTSDROP = 1643626701U;
        static const AkUniqueID TIMERENDGAME = 3486474161U;
    } // namespace EVENTS

    namespace STATES
    {
        namespace INGAME
        {
            static const AkUniqueID GROUP = 984691642U;

            namespace STATE
            {
                static const AkUniqueID INGAME = 984691642U;
                static const AkUniqueID INMENU = 3374585465U;
                static const AkUniqueID NONE = 748895195U;
            } // namespace STATE
        } // namespace INGAME

        namespace INMENU
        {
            static const AkUniqueID GROUP = 3374585465U;

            namespace STATE
            {
                static const AkUniqueID INMENU = 3374585465U;
                static const AkUniqueID INWAITINGROOM = 416437412U;
                static const AkUniqueID NONE = 748895195U;
            } // namespace STATE
        } // namespace INMENU

    } // namespace STATES

    namespace SWITCHES
    {
        namespace SWITCHEWEAPONSTYPE
        {
            static const AkUniqueID GROUP = 1861379221U;

            namespace SWITCH
            {
                static const AkUniqueID SWITCHEWEAPONSTYPEBAT = 3307155126U;
                static const AkUniqueID SWITCHEWEAPONSTYPECATHAMMER = 3771686185U;
                static const AkUniqueID SWITCHEWEAPONSTYPEPIPE = 3160374033U;
                static const AkUniqueID SWITCHEWEAPONSTYPESIGN = 1444194542U;
            } // namespace SWITCH
        } // namespace SWITCHEWEAPONSTYPE

        namespace SWITCHGAMESTATE
        {
            static const AkUniqueID GROUP = 3391824866U;

            namespace SWITCH
            {
                static const AkUniqueID SWITCHGAMESTATEINGAME = 3493037553U;
                static const AkUniqueID SWITCHGAMESTATEWAITING = 528386783U;
            } // namespace SWITCH
        } // namespace SWITCHGAMESTATE

    } // namespace SWITCHES

    namespace GAME_PARAMETERS
    {
        static const AkUniqueID RTPCGAMETIMER = 847161145U;
        static const AkUniqueID RTPCPLAYERVOICEPERHEALTH = 1693736946U;
        static const AkUniqueID RTPCSLASHSOUNDPERVELOCITY = 3236278648U;
        static const AkUniqueID RTPCWEAPONINHAND = 147779934U;
    } // namespace GAME_PARAMETERS

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID BLUNTBRAWLSOUNDBANKS = 3600579960U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID MASTER_AUDIO_BUS = 3803692087U;
        static const AkUniqueID MASTERVRAUDIOBUS = 4032808771U;
    } // namespace BUSSES

    namespace AUX_BUSSES
    {
        static const AkUniqueID MASTERVRAUXBUS = 1797866611U;
    } // namespace AUX_BUSSES

    namespace AUDIO_DEVICES
    {
        static const AkUniqueID NO_OUTPUT = 2317455096U;
        static const AkUniqueID SYSTEM = 3859886410U;
        static const AkUniqueID SYSTEMVR = 1998438890U;
    } // namespace AUDIO_DEVICES

}// namespace AK

#endif // __WWISE_IDS_H__
