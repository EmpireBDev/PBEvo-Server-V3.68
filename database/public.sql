/*
 Navicat Premium Data Transfer

 Source Server         : PostgreSQL
 Source Server Type    : PostgreSQL
 Source Server Version : 150002
 Source Host           : localhost:5432
 Source Catalog        : PointBlankN
 Source Schema         : public

 Target Server Type    : PostgreSQL
 Target Server Version : 150002
 File Encoding         : 65001

 Date: 08/12/2024 02:01:22
*/


-- ----------------------------
-- Sequence structure for accounts_seq
-- ----------------------------
DROP SEQUENCE IF EXISTS "public"."accounts_seq";
CREATE SEQUENCE "public"."accounts_seq" 
INCREMENT 1
MINVALUE  1
MAXVALUE 9223372036854775807
START 1
CACHE 1;

-- ----------------------------
-- Sequence structure for base_auto_ban_seq
-- ----------------------------
DROP SEQUENCE IF EXISTS "public"."base_auto_ban_seq";
CREATE SEQUENCE "public"."base_auto_ban_seq" 
INCREMENT 1
MINVALUE  1
MAXVALUE 9223372036854775807
START 1
CACHE 1;

-- ----------------------------
-- Sequence structure for base_ban_history_seq
-- ----------------------------
DROP SEQUENCE IF EXISTS "public"."base_ban_history_seq";
CREATE SEQUENCE "public"."base_ban_history_seq" 
INCREMENT 1
MINVALUE  1
MAXVALUE 9223372036854775807
START 1
CACHE 1;

-- ----------------------------
-- Sequence structure for base_nick_history_seq
-- ----------------------------
DROP SEQUENCE IF EXISTS "public"."base_nick_history_seq";
CREATE SEQUENCE "public"."base_nick_history_seq" 
INCREMENT 1
MINVALUE  1
MAXVALUE 9223372036854775807
START 1
CACHE 1;

-- ----------------------------
-- Sequence structure for base_redeem_history_seq
-- ----------------------------
DROP SEQUENCE IF EXISTS "public"."base_redeem_history_seq";
CREATE SEQUENCE "public"."base_redeem_history_seq" 
INCREMENT 1
MINVALUE  1
MAXVALUE 9223372036854775807
START 1
CACHE 1;

-- ----------------------------
-- Sequence structure for player_characters_seq
-- ----------------------------
DROP SEQUENCE IF EXISTS "public"."player_characters_seq";
CREATE SEQUENCE "public"."player_characters_seq" 
INCREMENT 1
MINVALUE  1
MAXVALUE 9223372036854775807
START 1
CACHE 1;

-- ----------------------------
-- Sequence structure for player_friends_seq
-- ----------------------------
DROP SEQUENCE IF EXISTS "public"."player_friends_seq";
CREATE SEQUENCE "public"."player_friends_seq" 
INCREMENT 1
MINVALUE  1
MAXVALUE 9223372036854775807
START 1
CACHE 1;

-- ----------------------------
-- Sequence structure for player_items_seq
-- ----------------------------
DROP SEQUENCE IF EXISTS "public"."player_items_seq";
CREATE SEQUENCE "public"."player_items_seq" 
INCREMENT 1
MINVALUE  1
MAXVALUE 9223372036854775807
START 1
CACHE 1;

-- ----------------------------
-- Sequence structure for player_messages_seq
-- ----------------------------
DROP SEQUENCE IF EXISTS "public"."player_messages_seq";
CREATE SEQUENCE "public"."player_messages_seq" 
INCREMENT 1
MINVALUE  1
MAXVALUE 9223372036854775807
START 1
CACHE 1;

-- ----------------------------
-- Sequence structure for system_clan_seq
-- ----------------------------
DROP SEQUENCE IF EXISTS "public"."system_clan_seq";
CREATE SEQUENCE "public"."system_clan_seq" 
INCREMENT 1
MINVALUE  1
MAXVALUE 9223372036854775807
START 1
CACHE 1;

-- ----------------------------
-- Sequence structure for system_events_visit_seq
-- ----------------------------
DROP SEQUENCE IF EXISTS "public"."system_events_visit_seq";
CREATE SEQUENCE "public"."system_events_visit_seq" 
INCREMENT 1
MINVALUE  1
MAXVALUE 9223372036854775807
START 1
CACHE 1;

-- ----------------------------
-- Table structure for accounts
-- ----------------------------
DROP TABLE IF EXISTS "public"."accounts";
CREATE TABLE "public"."accounts" (
  "username" varchar(16) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "password" varchar(16) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "player_id" int8 NOT NULL DEFAULT nextval('accounts_seq'::regclass),
  "email" varchar(255) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "age" int4 NOT NULL DEFAULT 0,
  "ip4_address" varchar(32) COLLATE "pg_catalog"."default" NOT NULL DEFAULT 0,
  "mac_address" macaddr NOT NULL DEFAULT '00:00:00:00:00:00'::macaddr,
  "nickname" varchar(16) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "nick_color" int4 NOT NULL DEFAULT 0,
  "rank" int4 NOT NULL DEFAULT 35,
  "experience" int4 NOT NULL DEFAULT 2414000,
  "gold" int4 NOT NULL DEFAULT 72000,
  "cash" int4 NOT NULL DEFAULT 1800,
  "pc_cafe" int4 NOT NULL DEFAULT 0,
  "access_level" int4 NOT NULL DEFAULT 0,
  "online" bool NOT NULL DEFAULT false,
  "tourney_level" int4 NOT NULL DEFAULT 0,
  "clan_id" int4 NOT NULL DEFAULT 0,
  "clan_access" int4 NOT NULL DEFAULT 0,
  "clan_date" int8 NOT NULL DEFAULT 0,
  "coupon_effect" int8 NOT NULL DEFAULT 0,
  "status" int8 NOT NULL DEFAULT '4294967295'::bigint,
  "last_rank_update" int8 NOT NULL DEFAULT 0,
  "ban_object_id" int8 NOT NULL DEFAULT 0,
  "last_login_date" int8 NOT NULL DEFAULT 0,
  "ribbon" int4 NOT NULL DEFAULT 10,
  "ensign" int4 NOT NULL DEFAULT 124,
  "medal" int4 NOT NULL DEFAULT 403,
  "master_medal" int4 NOT NULL DEFAULT 147,
  "mission_id1" int4 NOT NULL DEFAULT 1,
  "mission_id2" int4 NOT NULL DEFAULT 0,
  "mission_id3" int4 NOT NULL DEFAULT 0,
  "tags" int4 NOT NULL DEFAULT 0,
  "inventory_plus" int4 NOT NULL DEFAULT 0
)
;

-- ----------------------------
-- Records of accounts
-- ----------------------------
INSERT INTO "public"."accounts" VALUES ('xalgoz', 'casa1234', 51, '', 0, '26.134.207.164', '04:a0:08:85:1f:86', 'xAlgoz', 0, 53, 31598680, 72000, 1800, 2, 6, 'f', 0, 0, 0, 0, 0, 4294967295, 0, 0, 1412052050, 10, 124, 403, 147, 1, 0, 0, 0, 0);
INSERT INTO "public"."accounts" VALUES ('admin', 'qwerty', 47, '', 31, '26.134.207.164', '04:a0:08:85:1f:86', 'Garry', 0, 53, 32630296, 2015898, 136330, 2, 6, 'f', 7, 5, 1, 20190701, 71692424, 4294967295, 0, 0, 1412080146, 9, 875, 596, 852, 1, 0, 0, 811, 0);
INSERT INTO "public"."accounts" VALUES ('admin2', 'qwerty', 48, '', 31, '26.134.207.164', '04:a0:08:85:1f:86', 'Marry', 202, 54, 31599820, 1939122, 28780, 2, 5, 'f', 0, 0, 0, 0, 49152, 4294967295, 0, 0, 1412072357, 972, 711, 337, 744, 1, 0, 0, 500, 0);
INSERT INTO "public"."accounts" VALUES ('admin3', 'qwerty', 50, '', 0, '26.134.207.164', '04:a0:08:85:1f:86', 'Fishbourne', 0, 35, 2414000, 72000, 1600, 0, 0, 'f', 0, 0, 0, 0, 0, 4294967295, 0, 0, 1412052041, 0, 0, 0, 0, 1, 0, 0, 0, 0);

-- ----------------------------
-- Table structure for base_auto_ban
-- ----------------------------
DROP TABLE IF EXISTS "public"."base_auto_ban";
CREATE TABLE "public"."base_auto_ban" (
  "object_id" int8 NOT NULL DEFAULT nextval('base_auto_ban_seq'::regclass),
  "owner_id" int8 NOT NULL DEFAULT 0,
  "username" varchar(16) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "nickname" varchar(32) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "type" varchar(255) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "time" varchar(255) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "ip4_address" varchar(255) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "hack_type" varchar(255) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying
)
;

-- ----------------------------
-- Records of base_auto_ban
-- ----------------------------

-- ----------------------------
-- Table structure for base_ban_history
-- ----------------------------
DROP TABLE IF EXISTS "public"."base_ban_history";
CREATE TABLE "public"."base_ban_history" (
  "object_id" int8 NOT NULL DEFAULT nextval('base_ban_history_seq'::regclass),
  "owner_id" int8 NOT NULL DEFAULT 0,
  "type" varchar(255) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "value" varchar(255) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "reason" varchar(255) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "start_date" timestamp(6) NOT NULL DEFAULT '2000-01-01 00:00:00'::timestamp without time zone,
  "expire_date" timestamp(6) NOT NULL DEFAULT '2000-01-01 00:00:00'::timestamp without time zone
)
;

-- ----------------------------
-- Records of base_ban_history
-- ----------------------------

-- ----------------------------
-- Table structure for base_ban_hwid
-- ----------------------------
DROP TABLE IF EXISTS "public"."base_ban_hwid";
CREATE TABLE "public"."base_ban_hwid" (
  "hardware_id" varchar(255) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying
)
;

-- ----------------------------
-- Records of base_ban_hwid
-- ----------------------------

-- ----------------------------
-- Table structure for base_nick_history
-- ----------------------------
DROP TABLE IF EXISTS "public"."base_nick_history";
CREATE TABLE "public"."base_nick_history" (
  "object_id" int8 NOT NULL DEFAULT nextval('base_nick_history_seq'::regclass),
  "owner_id" int8 NOT NULL DEFAULT 0,
  "old_nick" varchar(32) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "new_nick" varchar(32) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "change_date" int8 NOT NULL DEFAULT 0,
  "motive" varchar(255) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying
)
;

-- ----------------------------
-- Records of base_nick_history
-- ----------------------------
INSERT INTO "public"."base_nick_history" VALUES (9, 48, '', 'Kravchenko', 2407120457, 'First nick choosed');
INSERT INTO "public"."base_nick_history" VALUES (11, 50, '', 'Fishbourne', 1410211951, 'First nick choosed');
INSERT INTO "public"."base_nick_history" VALUES (13, 51, '', 'xAlgoz', 1412052051, 'First nick choosed');
INSERT INTO "public"."base_nick_history" VALUES (8, 47, '', 'Garry', 2406201420, 'First nick choosed');
INSERT INTO "public"."base_nick_history" VALUES (12, 48, 'Kravchenko', 'Marry', 1412040811, 'Nickname changed (Item)');

-- ----------------------------
-- Table structure for base_redeem_history
-- ----------------------------
DROP TABLE IF EXISTS "public"."base_redeem_history";
CREATE TABLE "public"."base_redeem_history" (
  "object_id" int8 NOT NULL DEFAULT nextval('base_redeem_history_seq'::regclass),
  "owner_id" int8 NOT NULL DEFAULT 0,
  "used_token" varchar COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "used_count" int4 NOT NULL DEFAULT 0
)
;

-- ----------------------------
-- Records of base_redeem_history
-- ----------------------------
INSERT INTO "public"."base_redeem_history" VALUES (6, 47, 'CASHANDGOLD', 3);
INSERT INTO "public"."base_redeem_history" VALUES (11, 47, 'PBIC2015SERIES', 1);
INSERT INTO "public"."base_redeem_history" VALUES (9, 47, 'UZIPACKAGE', 2);

-- ----------------------------
-- Table structure for player_battlepass
-- ----------------------------
DROP TABLE IF EXISTS "public"."player_battlepass";
CREATE TABLE "public"."player_battlepass" (
  "owner_id" int8 NOT NULL DEFAULT 0,
  "bp_id" int4 NOT NULL DEFAULT 0,
  "premium" bool NOT NULL DEFAULT false,
  "total_points" int4 NOT NULL DEFAULT 0,
  "current_level" int4 NOT NULL DEFAULT 0,
  "finished_level" int4 NOT NULL DEFAULT 0,
  "finished_pab_level" int4 NOT NULL DEFAULT 0,
  "current_up_level" int4 NOT NULL DEFAULT 0
)
;

-- ----------------------------
-- Records of player_battlepass
-- ----------------------------

-- ----------------------------
-- Table structure for player_bonus
-- ----------------------------
DROP TABLE IF EXISTS "public"."player_bonus";
CREATE TABLE "public"."player_bonus" (
  "owner_id" int8 NOT NULL DEFAULT 0,
  "bonuses" int4 NOT NULL DEFAULT 0,
  "crosshair_color" int4 NOT NULL DEFAULT 0,
  "free_pass" int4 NOT NULL DEFAULT 0,
  "fake_rank" int4 NOT NULL DEFAULT 55,
  "fake_nick" varchar(16) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "muzzle_color" int4 NOT NULL DEFAULT 0
)
;

-- ----------------------------
-- Records of player_bonus
-- ----------------------------
INSERT INTO "public"."player_bonus" VALUES (48, 0, 0, 0, 55, '', 0);
INSERT INTO "public"."player_bonus" VALUES (47, 0, 4, 0, 55, '', 0);
INSERT INTO "public"."player_bonus" VALUES (50, 0, 0, 0, 55, '', 0);
INSERT INTO "public"."player_bonus" VALUES (51, 0, 0, 0, 55, '', 0);

-- ----------------------------
-- Table structure for player_characters
-- ----------------------------
DROP TABLE IF EXISTS "public"."player_characters";
CREATE TABLE "public"."player_characters" (
  "object_id" int8 NOT NULL DEFAULT nextval('player_characters_seq'::regclass),
  "owner_id" int8 NOT NULL DEFAULT 0,
  "id" int4 NOT NULL DEFAULT 0,
  "slot" int4 NOT NULL DEFAULT 255,
  "name" varchar(255) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "create_date" int8 NOT NULL DEFAULT 0,
  "playtime" int8 NOT NULL DEFAULT 0
)
;

-- ----------------------------
-- Records of player_characters
-- ----------------------------
INSERT INTO "public"."player_characters" VALUES (1074342825, 47, 601001, 0, 'Red Bulls', 2406201420, 0);
INSERT INTO "public"."player_characters" VALUES (1074343826, 47, 602002, 1, 'Acid Pool', 2406201420, 0);
INSERT INTO "public"."player_characters" VALUES (1074342834, 47, 601010, 2, 'Rica', 2406201420, 0);
INSERT INTO "public"."player_characters" VALUES (1074343835, 47, 602011, 3, 'Chou', 2406201420, 0);
INSERT INTO "public"."player_characters" VALUES (1074342893, 47, 601069, 4, 'Caiman Grey', 2406201420, 0);
INSERT INTO "public"."player_characters" VALUES (1074343894, 47, 602070, 5, 'Wolf', 2406201420, 0);
INSERT INTO "public"."player_characters" VALUES (183, 47, 602287, 6, 'Чероки-диверсантка', 2408161949, 0);
INSERT INTO "public"."player_characters" VALUES (184, 47, 601286, 7, 'Рика-бунтарка', 2408161949, 0);
INSERT INTO "public"."player_characters" VALUES (185, 47, 601244, 8, 'Купальный костюм Тарантула', 2408162130, 0);
INSERT INTO "public"."player_characters" VALUES (186, 47, 602177, 9, 'Купальный костюм Мираж', 2408162130, 0);
INSERT INTO "public"."player_characters" VALUES (1512, 48, 601001, 0, 'Red Bulls', 2407120457, 0);
INSERT INTO "public"."player_characters" VALUES (1513, 48, 602002, 1, 'Acid Pool', 2407120457, 0);
INSERT INTO "public"."player_characters" VALUES (201, 48, 602004, 2, 'Чероки', 1411291044, 0);
INSERT INTO "public"."player_characters" VALUES (202, 48, 601003, 3, 'Тарантула', 1411291044, 0);
INSERT INTO "public"."player_characters" VALUES (203, 47, 632660, 10, 'Ковон', 1411291112, 0);
INSERT INTO "public"."player_characters" VALUES (204, 47, 664661, 11, 'Ковон', 1411291112, 0);
INSERT INTO "public"."player_characters" VALUES (205, 48, 632660, 4, 'Ковон', 1412040752, 0);
INSERT INTO "public"."player_characters" VALUES (206, 51, 601001, 0, 'Red Bulls', 1412052051, 0);
INSERT INTO "public"."player_characters" VALUES (207, 51, 602002, 1, 'Acid Pool', 1412052051, 0);
INSERT INTO "public"."player_characters" VALUES (199, 50, 601001, 0, 'Red Bulls', 1410211951, 0);
INSERT INTO "public"."player_characters" VALUES (200, 50, 602002, 1, 'Acid Pool', 1410211951, 0);

-- ----------------------------
-- Table structure for player_configs
-- ----------------------------
DROP TABLE IF EXISTS "public"."player_configs";
CREATE TABLE "public"."player_configs" (
  "owner_id" int8 NOT NULL DEFAULT 0,
  "configs" int4 NOT NULL DEFAULT 55,
  "show_blood" int4 NOT NULL DEFAULT 3,
  "crosshair" int4 NOT NULL DEFAULT 2,
  "hand_pos" int4 NOT NULL DEFAULT 0,
  "audio_sfx" int4 NOT NULL DEFAULT 100,
  "audio_bgm" int4 NOT NULL DEFAULT 50,
  "audio_enable" int4 NOT NULL DEFAULT 7,
  "sensitivity" int4 NOT NULL DEFAULT 50,
  "pov_size" int4 NOT NULL DEFAULT 80,
  "invert_mouse" int4 NOT NULL DEFAULT 0,
  "enable_invite" int4 NOT NULL DEFAULT 0,
  "enable_whisper" int4 NOT NULL DEFAULT 0,
  "macro_enable" int4 NOT NULL DEFAULT 31,
  "macro1" varchar(255) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "macro2" varchar(255) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "macro3" varchar(255) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "macro4" varchar(255) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "macro5" varchar(255) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "keyboard_keys" bytea NOT NULL DEFAULT '\x'::bytea,
  "nations" int4 NOT NULL DEFAULT 0
)
;

-- ----------------------------
-- Records of player_configs
-- ----------------------------
INSERT INTO "public"."player_configs" VALUES (50, 55, 3, 2, 0, 100, 50, 7, 22, 80, 0, 0, 0, 31, '', '', '', '', '', E'\\000\\012\\000\\000\\000\\000\\015\\000\\000\\000\\000 \\000\\000\\000\\000\\034\\000\\000\\000\\000,\\000\\000\\000\\000(\\000\\000\\000\\000&\\000\\000\\000\\000\\017\\000\\000\\000\\001\\001\\000\\000\\000\\001\\002\\000\\000\\000\\000\\033\\000\\000\\000\\000\\035\\000\\000\\000\\000\\001\\000\\000\\000\\000\\002\\000\\000\\000\\000\\003\\000\\000\\000\\000\\004\\000\\000\\000\\000\\005\\000\\000\\000\\000\\006\\000\\000\\000\\000\\032\\000\\000\\000\\001\\000\\000\\000\\020\\001\\000\\000\\000 \\000\\020\\000\\000\\000\\0007\\000\\000\\000\\377\\026\\000\\000\\000\\000\\\\\\000\\000\\000\\000[\\000\\000\\000\\000%\\000\\000\\000\\000@\\000\\000\\000\\000A\\000\\000\\000\\000\\025\\000\\000\\000\\000\\377\\377\\377\\377\\000#\\000\\000\\000\\000!\\000\\000\\000\\000\\014\\000\\000\\000\\000\\016\\000\\000\\000\\0001\\000\\000\\000\\0002\\000\\000\\000\\000F\\000\\000\\000\\000B\\000\\000\\000\\000\\013\\000\\000\\000\\000:\\000\\000\\000\\377"\\000\\000\\000\\000\\377\\377\\377\\377\\000\\377\\377\\377\\377\\000\\377\\377\\377\\377\\000\\377\\377\\377\\377\\000\\377\\377\\377\\377\\000"\\000\\000\\000', 0);
INSERT INTO "public"."player_configs" VALUES (51, 55, 3, 2, 0, 100, 50, 7, 50, 80, 0, 0, 0, 31, '', '', '', '', '', E'\\000\\012\\000\\000\\000\\000\\015\\000\\000\\000\\000 \\000\\000\\000\\000\\034\\000\\000\\000\\000,\\000\\000\\000\\000(\\000\\000\\000\\000&\\000\\000\\000\\000\\017\\000\\000\\000\\001\\001\\000\\000\\000\\001\\002\\000\\000\\000\\000\\033\\000\\000\\000\\000\\035\\000\\000\\000\\000\\001\\000\\000\\000\\000\\002\\000\\000\\000\\000\\003\\000\\000\\000\\000\\004\\000\\000\\000\\000\\005\\000\\000\\000\\000\\006\\000\\000\\000\\000\\032\\000\\000\\000\\001\\000\\000\\000\\020\\001\\000\\000\\000 \\000\\020\\000\\000\\000\\0007\\000\\000\\000\\000\\026\\000\\000\\000\\000\\\\\\000\\000\\000\\000[\\000\\000\\000\\000%\\000\\000\\000\\000@\\000\\000\\000\\000A\\000\\000\\000\\000\\025\\000\\000\\000\\000\\377\\377\\377\\377\\000#\\000\\000\\000\\000!\\000\\000\\000\\000\\014\\000\\000\\000\\000\\016\\000\\000\\000\\0001\\000\\000\\000\\0002\\000\\000\\000\\000F\\000\\000\\000\\000B\\000\\000\\000\\000\\013\\000\\000\\000\\000:\\000\\000\\000\\000"\\000\\000\\000\\000\\377\\377\\377\\377\\000\\377\\377\\377\\377\\000\\377\\377\\377\\377\\000\\377\\377\\377\\377\\000\\377\\377\\377\\377\\000"\\000\\000\\000', 0);
INSERT INTO "public"."player_configs" VALUES (47, 55, 3, 2, 0, 100, 50, 6, 22, 80, 0, 0, 0, 31, '', '', '', '', '', E'\\000\\012\\000\\000\\000\\000\\015\\000\\000\\000\\000 \\000\\000\\000\\000\\034\\000\\000\\000\\000,\\000\\000\\000\\000(\\000\\000\\000\\000&\\000\\000\\000\\000\\017\\000\\000\\000\\001\\001\\000\\000\\000\\001\\002\\000\\000\\000\\000\\033\\000\\000\\000\\000\\035\\000\\000\\000\\000\\001\\000\\000\\000\\000\\002\\000\\000\\000\\000\\003\\000\\000\\000\\000\\004\\000\\000\\000\\000\\005\\000\\000\\000\\000\\006\\000\\000\\000\\000\\032\\000\\000\\000\\001\\000\\000\\000\\020\\001\\000\\000\\000 \\000\\020\\000\\000\\000\\0007\\000\\000\\000\\377\\000\\000\\000\\000\\000\\\\\\000\\000\\000\\000[\\000\\000\\000\\000%\\000\\000\\000\\000@\\000\\000\\000\\000A\\000\\000\\000\\000\\025\\000\\000\\000\\000\\377\\377\\377\\377\\000#\\000\\000\\000\\000!\\000\\000\\000\\000\\014\\000\\000\\000\\000\\016\\000\\000\\000\\0001\\000\\000\\000\\0002\\000\\000\\000\\000F\\000\\000\\000\\000B\\000\\000\\000\\000\\013\\000\\000\\000\\000:\\000\\000\\000\\377"\\000\\000\\000\\000\\377\\377\\377\\377\\000\\377\\377\\377\\377\\000\\377\\377\\377\\377\\000\\377\\377\\377\\377\\000\\377\\377\\377\\377\\000"\\000\\000\\000', 0);
INSERT INTO "public"."player_configs" VALUES (48, 55, 3, 2, 0, 100, 50, 6, 22, 80, 0, 0, 0, 31, '', '', '', '', '', E'\\000\\012\\000\\000\\000\\000\\015\\000\\000\\000\\000 \\000\\000\\000\\000\\034\\000\\000\\000\\000,\\000\\000\\000\\000(\\000\\000\\000\\000&\\000\\000\\000\\000\\017\\000\\000\\000\\001\\001\\000\\000\\000\\001\\002\\000\\000\\000\\000\\033\\000\\000\\000\\000\\035\\000\\000\\000\\000\\001\\000\\000\\000\\000\\002\\000\\000\\000\\000\\003\\000\\000\\000\\000\\004\\000\\000\\000\\000\\005\\000\\000\\000\\000\\006\\000\\000\\000\\000\\032\\000\\000\\000\\001\\000\\000\\000\\020\\001\\000\\000\\000 \\000\\020\\000\\000\\000\\0007\\000\\000\\000\\377\\000\\000\\000\\000\\000\\\\\\000\\000\\000\\000[\\000\\000\\000\\000%\\000\\000\\000\\000@\\000\\000\\000\\000A\\000\\000\\000\\000\\025\\000\\000\\000\\000\\377\\377\\377\\377\\000#\\000\\000\\000\\000!\\000\\000\\000\\000\\014\\000\\000\\000\\000\\016\\000\\000\\000\\0001\\000\\000\\000\\0002\\000\\000\\000\\000F\\000\\000\\000\\000B\\000\\000\\000\\000\\013\\000\\000\\000\\000:\\000\\000\\000\\377"\\000\\000\\000\\000\\377\\377\\377\\377\\000\\377\\377\\377\\377\\000\\377\\377\\377\\377\\000\\377\\377\\377\\377\\000\\377\\377\\377\\377\\000"\\000\\000\\000', 0);

-- ----------------------------
-- Table structure for player_equipments
-- ----------------------------
DROP TABLE IF EXISTS "public"."player_equipments";
CREATE TABLE "public"."player_equipments" (
  "owner_id" int8 NOT NULL DEFAULT 0,
  "weapon_primary" int4 NOT NULL DEFAULT 103004,
  "weapon_secondary" int4 NOT NULL DEFAULT 202003,
  "weapon_melee" int4 NOT NULL DEFAULT 301001,
  "weapon_explosive" int4 NOT NULL DEFAULT 407001,
  "weapon_special" int4 NOT NULL DEFAULT 508001,
  "chara_red_side" int4 NOT NULL DEFAULT 601001,
  "chara_blue_side" int4 NOT NULL DEFAULT 602002,
  "dino_item_chara" int4 NOT NULL DEFAULT 1500511,
  "part_head" int4 NOT NULL DEFAULT 1000700000,
  "part_face" int4 NOT NULL DEFAULT 1000800000,
  "part_jacket" int4 NOT NULL DEFAULT 1000900000,
  "part_pocket" int4 NOT NULL DEFAULT 1001000000,
  "part_glove" int4 NOT NULL DEFAULT 1001100000,
  "part_belt" int4 NOT NULL DEFAULT 1001200000,
  "part_holster" int4 NOT NULL DEFAULT 1001300000,
  "part_skin" int4 NOT NULL DEFAULT 1001400000,
  "beret_item_part" int4 NOT NULL DEFAULT 0,
  "accesory_id" int4 NOT NULL DEFAULT 0,
  "spray_id" int4 NOT NULL DEFAULT 0,
  "namecard_id" int4 NOT NULL DEFAULT 0
)
;

-- ----------------------------
-- Records of player_equipments
-- ----------------------------
INSERT INTO "public"."player_equipments" VALUES (48, 105431, 202003, 301297, 407001, 527001, 601003, 602004, 1500513, 1000700000, 1000800000, 1000900000, 1001000000, 1001100000, 1001200000, 1001300000, 1001400000, 2700013, 3000045, 0, 3400153);
INSERT INTO "public"."player_equipments" VALUES (50, 103324, 202003, 301001, 407001, 508001, 601001, 602002, 1500513, 700125, 1000800000, 1000900000, 1001000000, 1001100000, 1001200000, 1001300000, 1001400000, 0, 0, 0, 0);
INSERT INTO "public"."player_equipments" VALUES (51, 103004, 202003, 301001, 407001, 508001, 601001, 602002, 1500511, 1000700000, 1000800000, 1000900000, 1001000000, 1001100000, 1001200000, 1001300000, 1001400000, 0, 0, 0, 0);
INSERT INTO "public"."player_equipments" VALUES (47, 103537, 202003, 301001, 407001, 508001, 632660, 664661, 1500513, 702256, 1000800000, 1000900000, 1001000000, 1001100000, 1001200000, 1001300000, 1001400000, 2700013, 3000043, 3200187, 3400147);

-- ----------------------------
-- Table structure for player_events
-- ----------------------------
DROP TABLE IF EXISTS "public"."player_events";
CREATE TABLE "public"."player_events" (
  "owner_id" int8 NOT NULL DEFAULT 0,
  "last_visit_event_id" int4 NOT NULL DEFAULT 0,
  "last_visit_sequence1" int4 NOT NULL DEFAULT 0,
  "last_visit_sequence2" int4 NOT NULL DEFAULT 0,
  "next_visit_date" int8 NOT NULL DEFAULT 0,
  "last_xmas_reward_date" int8 NOT NULL DEFAULT 0,
  "last_playtime_date" int8 NOT NULL DEFAULT 0,
  "last_playtime_value" int4 NOT NULL DEFAULT 0,
  "last_playtime_finish" int4 NOT NULL DEFAULT 0,
  "last_login_date" int8 NOT NULL DEFAULT 0,
  "last_quest_date" int8 NOT NULL DEFAULT 0,
  "last_quest_finish" int4 NOT NULL DEFAULT 0
)
;

-- ----------------------------
-- Records of player_events
-- ----------------------------
INSERT INTO "public"."player_events" VALUES (48, 1, 3, 3, 140921, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO "public"."player_events" VALUES (47, 1, 7, 7, 140922, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO "public"."player_events" VALUES (50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO "public"."player_events" VALUES (51, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

-- ----------------------------
-- Table structure for player_friends
-- ----------------------------
DROP TABLE IF EXISTS "public"."player_friends";
CREATE TABLE "public"."player_friends" (
  "object_id" int8 NOT NULL DEFAULT nextval('player_friends_seq'::regclass),
  "owner_id" int8 NOT NULL DEFAULT 0,
  "id" int8 NOT NULL DEFAULT 0,
  "state" int4 NOT NULL DEFAULT 0,
  "removed" bool NOT NULL DEFAULT false
)
;

-- ----------------------------
-- Records of player_friends
-- ----------------------------
INSERT INTO "public"."player_friends" VALUES (26, 48, 47, 0, 'f');
INSERT INTO "public"."player_friends" VALUES (24, 47, 48, 0, 'f');

-- ----------------------------
-- Table structure for player_items
-- ----------------------------
DROP TABLE IF EXISTS "public"."player_items";
CREATE TABLE "public"."player_items" (
  "object_id" int8 NOT NULL DEFAULT nextval('player_items_seq'::regclass),
  "owner_id" int8 NOT NULL DEFAULT 0,
  "id" int4 NOT NULL DEFAULT 0,
  "name" varchar(255) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "count" int8 NOT NULL DEFAULT 0,
  "equip" int4 NOT NULL DEFAULT 0
)
;

-- ----------------------------
-- Records of player_items
-- ----------------------------
INSERT INTO "public"."player_items" VALUES (1536, 48, 105006, 'Dragunov G. (10 Qty)', 100, 1);
INSERT INTO "public"."player_items" VALUES (1534, 48, 103014, 'SG 550 S. (10 Qty)', 100, 1);
INSERT INTO "public"."player_items" VALUES (1537, 48, 213001, 'P99 HAK (10 Qty)', 100, 1);
INSERT INTO "public"."player_items" VALUES (1538, 48, 104007, 'MP5K G. (10 Qty)', 100, 1);
INSERT INTO "public"."player_items" VALUES (1584, 47, 105099, 'Cheytac M200 Couple Breaker (24 hours)', 259200, 1);
INSERT INTO "public"."player_items" VALUES (1590, 47, 104251, 'OA-93 XMAS 2015 (24 hours)', 86400, 1);
INSERT INTO "public"."player_items" VALUES (1539, 48, 104009, 'Spectre W. (10 Qty)', 100, 1);
INSERT INTO "public"."player_items" VALUES (1591, 47, 104249, 'Kriss S.V XMAS 2015 (24 hours)', 86400, 1);
INSERT INTO "public"."player_items" VALUES (1651, 48, 104011, 'P90 Ext. (10 Qty)', 70, 1);
INSERT INTO "public"."player_items" VALUES (1512, 48, 601001, 'Red Bulls', 1, 3);
INSERT INTO "public"."player_items" VALUES (1513, 48, 602002, 'Acid Pool', 1, 3);
INSERT INTO "public"."player_items" VALUES (1595, 47, 103092, 'AK SOPMOD R (24 hours)', 86400, 1);
INSERT INTO "public"."player_items" VALUES (1542, 48, 103013, 'G36C Ext. (10 Qty)', 100, 1);
INSERT INTO "public"."player_items" VALUES (1543, 48, 301004, 'Amok Kukri (10 Qty)', 100, 1);
INSERT INTO "public"."player_items" VALUES (1544, 48, 105024, 'PSG1 G (10 Qty)', 100, 1);
INSERT INTO "public"."player_items" VALUES (1545, 48, 214001, 'Dual Handgun (10 Qty)', 100, 1);
INSERT INTO "public"."player_items" VALUES (1546, 48, 214002, 'Dual D-Eagle (10 Qty)', 100, 1);
INSERT INTO "public"."player_items" VALUES (1549, 48, 106003, 'SPAS-15 (10 Qty)', 100, 1);
INSERT INTO "public"."player_items" VALUES (1424, 47, 602011, 'Chou (100 qty)', 83, 1);
INSERT INTO "public"."player_items" VALUES (1550, 48, 103036, 'AUG A3 (10 Qty)', 100, 1);
INSERT INTO "public"."player_items" VALUES (1827, 50, 104359, 'OA-93 Beyond (3 days)', 259200, 1);
INSERT INTO "public"."player_items" VALUES (1421, 47, 601001, 'Red Bulls', 1, 3);
INSERT INTO "public"."player_items" VALUES (1828, 50, 105199, 'Cheytac M200 Beyond (3 days)', 259200, 1);
INSERT INTO "public"."player_items" VALUES (1495, 47, 105200, 'AS-50 Beyond (3 days)', 259200, 1);
INSERT INTO "public"."player_items" VALUES (1494, 47, 105199, 'Cheytac M200 Beyond (3 days)', 259200, 1);
INSERT INTO "public"."player_items" VALUES (1496, 47, 106090, 'M1887 Beyond (3 days)', 259200, 1);
INSERT INTO "public"."player_items" VALUES (1830, 50, 106090, 'M1887 Beyond (3 days)', 259200, 1);
INSERT INTO "public"."player_items" VALUES (1422, 47, 602002, 'Acid Pool', 1, 3);
INSERT INTO "public"."player_items" VALUES (1652, 47, 103014, 'SG 550 S. (10 Qty)', 10, 1);
INSERT INTO "public"."player_items" VALUES (1425, 47, 601069, 'Caiman Grey (100 qty)', 100, 1);
INSERT INTO "public"."player_items" VALUES (1426, 47, 602070, 'Wolf (100 qty)', 100, 1);
INSERT INTO "public"."player_items" VALUES (1654, 47, 105006, 'Dragunov G. (10 Qty)', 10, 1);
INSERT INTO "public"."player_items" VALUES (1622, 47, 602177, 'Vacance 2017 Hide (24 hours)', 86400, 1);
INSERT INTO "public"."player_items" VALUES (1526, 47, 106042, 'Zombie Slayer (24 hours)', 86400, 1);
INSERT INTO "public"."player_items" VALUES (1831, 50, 315016, 'Dual Bone Knife Beyond (3 days)', 259200, 1);
INSERT INTO "public"."player_items" VALUES (1498, 47, 800357, 'Mask Beyond (3 days)', 259200, 1);
INSERT INTO "public"."player_items" VALUES (1423, 47, 601010, 'Rica (100 qty)', 69, 1);
INSERT INTO "public"."player_items" VALUES (1666, 47, 301004, 'Amok Kukri (10 Qty)', 10, 1);
INSERT INTO "public"."player_items" VALUES (1669, 47, 103015, 'AK SOPMOD (10 Qty)', 10, 1);
INSERT INTO "public"."player_items" VALUES (1671, 47, 105005, 'L115A1 (10 Qty)', 10, 1);
INSERT INTO "public"."player_items" VALUES (1673, 47, 2700014, 'Beret Title Assault', 1, 3);
INSERT INTO "public"."player_items" VALUES (1675, 47, 2700015, 'Beret Title Handgun', 1, 3);
INSERT INTO "public"."player_items" VALUES (1677, 47, 2700018, 'Beret Title Shotgun', 1, 3);
INSERT INTO "public"."player_items" VALUES (1833, 50, 601001, 'Red Bulls', 1, 3);
INSERT INTO "public"."player_items" VALUES (1548, 48, 527001, 'WP Smoke (10 Qty)', 97, 1);
INSERT INTO "public"."player_items" VALUES (1845, 47, 632660, 'Gwoon Office FR (720 hours)', 1412291112, 2);
INSERT INTO "public"."player_items" VALUES (1556, 48, 2700016, 'Beret Title Sniper', 1, 3);
INSERT INTO "public"."player_items" VALUES (1558, 48, 2700017, 'Beret Title Submachine', 1, 3);
INSERT INTO "public"."player_items" VALUES (1552, 48, 301007, 'Mini Axe (10 Qty)', 100, 1);
INSERT INTO "public"."player_items" VALUES (1554, 48, 104013, 'Kriss S.V (10 Qty)', 100, 1);
INSERT INTO "public"."player_items" VALUES (1587, 47, 104170, 'Kriss S.V Couple Breaker (24 hours)', 172800, 1);
INSERT INTO "public"."player_items" VALUES (1593, 47, 103241, 'AUG A3 XMAS 2015 (24 hours)', 86400, 1);
INSERT INTO "public"."player_items" VALUES (1859, 47, 103537, 'AUG A3 Lovely Valentine (720 hours)', 1501061349, 2);
INSERT INTO "public"."player_items" VALUES (1655, 47, 213001, 'P99 HAK (10 Qty)', 10, 1);
INSERT INTO "public"."player_items" VALUES (1657, 47, 104009, 'Spectre W. (10 Qty)', 10, 1);
INSERT INTO "public"."player_items" VALUES (1659, 47, 106004, '870MCS W. (10 Qty)', 10, 1);
INSERT INTO "public"."player_items" VALUES (1663, 47, 214001, 'Dual Handgun (10 Qty)', 10, 1);
INSERT INTO "public"."player_items" VALUES (1614, 47, 602287, 'General Hide (24 hours)', 86400, 1);
INSERT INTO "public"."player_items" VALUES (1615, 47, 601286, 'General Viper Red (24 hours)', 86400, 1);
INSERT INTO "public"."player_items" VALUES (1551, 48, 103015, 'AK SOPMOD (10 Qty)', 95, 1);
INSERT INTO "public"."player_items" VALUES (1541, 48, 106004, '870MCS W. (10 Qty)', 99, 1);
INSERT INTO "public"."player_items" VALUES (1658, 47, 407002, 'C-5 (10 Qty)', 9, 1);
INSERT INTO "public"."player_items" VALUES (1535, 48, 315001, 'Dual Knife (10 Qty)', 71, 1);
INSERT INTO "public"."player_items" VALUES (1843, 48, 602004, 'Keen Eyes (100 qty)', 96, 1);
INSERT INTO "public"."player_items" VALUES (1846, 47, 664661, 'Gwoon Office CT (720 hours)', 1412291112, 2);
INSERT INTO "public"."player_items" VALUES (1656, 47, 104007, 'MP5K G. (10 Qty)', 8, 1);
INSERT INTO "public"."player_items" VALUES (1662, 47, 104011, 'P90 Ext. (10 Qty)', 6, 1);
INSERT INTO "public"."player_items" VALUES (1540, 48, 407002, 'C-5 (10 Qty)', 81, 1);
INSERT INTO "public"."player_items" VALUES (1855, 47, 103711, 'Aug A3 Golden Phoenix (720 hours)', 2592000, 1);
INSERT INTO "public"."player_items" VALUES (1660, 47, 106003, 'SPAS-15 (10 Qty)', 9, 1);
INSERT INTO "public"."player_items" VALUES (1856, 47, 105377, 'BORA12 Gold (720 hours)', 2592000, 1);
INSERT INTO "public"."player_items" VALUES (1851, 47, 103712, 'SC-2010 Golden Phoenix (168 hours)', 1412100932, 2);
INSERT INTO "public"."player_items" VALUES (1857, 47, 105485, 'Cheytac Golden Phoenix (720 hours)', 2592000, 1);
INSERT INTO "public"."player_items" VALUES (1858, 47, 106231, 'ZombieSlayer Golden Phoenix (720 hours)', 2592000, 1);
INSERT INTO "public"."player_items" VALUES (1860, 47, 103538, 'SC 2010 Lovely Valentine (720 hours)', 2592000, 1);
INSERT INTO "public"."player_items" VALUES (1861, 47, 105369, 'Cheytac M200 Lovely Valentine (720 hours)', 2592000, 1);
INSERT INTO "public"."player_items" VALUES (1863, 48, 103643, 'M4 Azure Dragon (720 hours)', 1501030738, 2);
INSERT INTO "public"."player_items" VALUES (1864, 48, 301276, 'TrenchKnife (720 hours)', 1501030738, 2);
INSERT INTO "public"."player_items" VALUES (1866, 48, 1600027, 'Quick Reload Weapon (30 days) [Active]', 1501030738, 2);
INSERT INTO "public"."player_items" VALUES (1869, 48, 632660, 'Gwoon Office FR (720 hours)', 1501030752, 2);
INSERT INTO "public"."player_items" VALUES (1872, 48, 1600080, 'Instant Respawn (30 days) [Active]', 1501030752, 2);
INSERT INTO "public"."player_items" VALUES (1825, 50, 103324, 'AUG A3 Beyond (3 days)', 1412082042, 2);
INSERT INTO "public"."player_items" VALUES (1665, 47, 105024, 'PSG1 G (10 Qty)', 10, 1);
INSERT INTO "public"."player_items" VALUES (1667, 47, 103013, 'G36C Ext. (10 Qty)', 10, 1);
INSERT INTO "public"."player_items" VALUES (1670, 47, 301007, 'Mini Axe (10 Qty)', 10, 1);
INSERT INTO "public"."player_items" VALUES (1674, 47, 2700016, 'Beret Title Sniper', 1, 3);
INSERT INTO "public"."player_items" VALUES (1676, 47, 2700017, 'Beret Title Submachine', 1, 3);
INSERT INTO "public"."player_items" VALUES (1826, 50, 104357, 'Kriss S.V Beyond (3 days)', 259200, 1);
INSERT INTO "public"."player_items" VALUES (1829, 50, 105200, 'AS-50 Beyond (3 days)', 259200, 1);
INSERT INTO "public"."player_items" VALUES (1832, 50, 800357, 'Mask Beyond (3 days)', 259200, 1);
INSERT INTO "public"."player_items" VALUES (1834, 50, 602002, 'Acid Pool', 1, 3);
INSERT INTO "public"."player_items" VALUES (1868, 48, 1600026, 'Quick Change Weapon (30 days) [Active]', 1501030738, 2);
INSERT INTO "public"."player_items" VALUES (1555, 48, 2700014, 'Beret Title Assault', 1, 3);
INSERT INTO "public"."player_items" VALUES (1557, 48, 2700015, 'Beret Title Handgun', 1, 3);
INSERT INTO "public"."player_items" VALUES (1559, 48, 2700018, 'Beret Title Shotgun', 1, 3);
INSERT INTO "public"."player_items" VALUES (1589, 47, 105138, 'Cheytac M200 XMAS 2015 (24 hours)', 86400, 1);
INSERT INTO "public"."player_items" VALUES (1600, 47, 104123, 'Kriss S.V R (24 hours)', 86400, 1);
INSERT INTO "public"."player_items" VALUES (1611, 47, 103144, 'AUG A3 R (24 hours)', 86400, 1);
INSERT INTO "public"."player_items" VALUES (1616, 47, 103154, 'SC-2010 Gold (24 hours)', 86400, 1);
INSERT INTO "public"."player_items" VALUES (1553, 48, 105005, 'L115A1 (10 Qty)', 100, 1);
INSERT INTO "public"."player_items" VALUES (1621, 47, 601244, 'Vacance 2017 Tarantula (24 hours)', 86400, 1);
INSERT INTO "public"."player_items" VALUES (1705, 47, 2800001, 'Dragon BattleBox (1 qty)', 1, 1);
INSERT INTO "public"."player_items" VALUES (1847, 47, 702256, 'Wave Hair (720 hours)', 1412291113, 2);
INSERT INTO "public"."player_items" VALUES (1734, 47, 104298, 'Kriss S.V Alien (24 hours)', 86400, 1);
INSERT INTO "public"."player_items" VALUES (1908, 50, 700134, 'Red Bulls Egg Mask (24 hours)', 1412062044, 2);
INSERT INTO "public"."player_items" VALUES (1672, 47, 104013, 'Kriss S.V (10 Qty)', 9, 1);
INSERT INTO "public"."player_items" VALUES (1862, 47, 3400147, '11th Anniversary Namecard (168 hours)', 1412101235, 2);
INSERT INTO "public"."player_items" VALUES (1765, 47, 118013, 'Dual Uzi Mini Gold (24 hours)', 604800, 1);
INSERT INTO "public"."player_items" VALUES (1766, 47, 202023, 'IMI Uzi9 (24 hours)', 604800, 1);
INSERT INTO "public"."player_items" VALUES (1653, 47, 315001, 'Dual Knife (10 Qty)', 8, 1);
INSERT INTO "public"."player_items" VALUES (1844, 48, 601003, 'Tarantula (100 qty)', 98, 1);
INSERT INTO "public"."player_items" VALUES (1870, 48, 3400153, 'Adana Namecard (168 hours)', 1412110752, 2);
INSERT INTO "public"."player_items" VALUES (1873, 47, 3200187, 'COVID-19 Spray (168 hours)', 1412110753, 2);
INSERT INTO "public"."player_items" VALUES (1875, 48, 103711, 'Aug A3 Golden Phoenix (720 hours)', 1501030800, 2);
INSERT INTO "public"."player_items" VALUES (1754, 47, 104218, 'P90 Ext PBIC 2015 (24 hours)', 172800, 1);
INSERT INTO "public"."player_items" VALUES (1755, 47, 105120, 'Cheytac M200 PBIC 2015 (24 hours)', 172800, 1);
INSERT INTO "public"."player_items" VALUES (1756, 47, 106058, 'M1887 PBIC 2015 (24 hours)', 172800, 1);
INSERT INTO "public"."player_items" VALUES (1877, 48, 1600006, 'Name Color [Active]', 1501030808, 2);
INSERT INTO "public"."player_items" VALUES (1878, 48, 301297, 'GH5007 PinkyPig (720 hours)', 1501030810, 2);
INSERT INTO "public"."player_items" VALUES (1744, 47, 105173, 'Cheytac M200 Alien (24 hours)', 86400, 1);
INSERT INTO "public"."player_items" VALUES (1879, 48, 105431, 'KAR98 DarkCupid (720 hours)', 1501030810, 2);
INSERT INTO "public"."player_items" VALUES (1880, 48, 3000045, 'Aquarium (Front) Accessory (168 hours)', 1412110811, 2);
INSERT INTO "public"."player_items" VALUES (1748, 47, 700123, 'Mask Alien - Blue Eye (24 hours)', 86400, 1);
INSERT INTO "public"."player_items" VALUES (1882, 50, 103014, 'SG 550 S. (10 Qty)', 10, 1);
INSERT INTO "public"."player_items" VALUES (1883, 50, 315001, 'Dual Knife (10 Qty)', 10, 1);
INSERT INTO "public"."player_items" VALUES (1884, 50, 105006, 'Dragunov G. (10 Qty)', 10, 1);
INSERT INTO "public"."player_items" VALUES (1885, 50, 213001, 'P99 HAK (10 Qty)', 10, 1);
INSERT INTO "public"."player_items" VALUES (1886, 50, 104007, 'MP5K G. (10 Qty)', 10, 1);
INSERT INTO "public"."player_items" VALUES (1887, 50, 104009, 'Spectre W. (10 Qty)', 10, 1);
INSERT INTO "public"."player_items" VALUES (1888, 50, 407002, 'C-5 (10 Qty)', 10, 1);
INSERT INTO "public"."player_items" VALUES (1889, 50, 106004, '870MCS W. (10 Qty)', 10, 1);
INSERT INTO "public"."player_items" VALUES (1758, 47, 301104, 'Keris PBIC 2015 (24 hours)', 172800, 1);
INSERT INTO "public"."player_items" VALUES (1759, 47, 800299, 'Mask PBIC 2015 Premium (24 hours)', 172800, 1);
INSERT INTO "public"."player_items" VALUES (1760, 47, 118004, 'Dual Uzi (24 hours)', 604800, 1);
INSERT INTO "public"."player_items" VALUES (1761, 47, 118005, 'Dual Uzi Mini (24 hours)', 604800, 1);
INSERT INTO "public"."player_items" VALUES (1762, 47, 118006, 'Dual Uzi Micro (24 hours)', 604800, 1);
INSERT INTO "public"."player_items" VALUES (1763, 47, 118009, 'Dual Uzi Micro Silencer (24 hours)', 604800, 1);
INSERT INTO "public"."player_items" VALUES (1890, 50, 103013, 'G36C Ext. (10 Qty)', 10, 1);
INSERT INTO "public"."player_items" VALUES (1891, 50, 301004, 'Amok Kukri (10 Qty)', 10, 1);
INSERT INTO "public"."player_items" VALUES (1892, 50, 105024, 'PSG1 G (10 Qty)', 10, 1);
INSERT INTO "public"."player_items" VALUES (1893, 50, 214001, 'Dual Handgun (10 Qty)', 10, 1);
INSERT INTO "public"."player_items" VALUES (1894, 50, 214002, 'Dual D-Eagle (10 Qty)', 10, 1);
INSERT INTO "public"."player_items" VALUES (1895, 50, 104011, 'P90 Ext. (10 Qty)', 10, 1);
INSERT INTO "public"."player_items" VALUES (1896, 50, 527001, 'WP Smoke (10 Qty)', 10, 1);
INSERT INTO "public"."player_items" VALUES (1779, 48, 702254, 'Updo Hair (720 hours)', 1412121305, 2);
INSERT INTO "public"."player_items" VALUES (1897, 50, 106003, 'SPAS-15 (10 Qty)', 10, 1);
INSERT INTO "public"."player_items" VALUES (1898, 50, 103036, 'AUG A3 (10 Qty)', 10, 1);
INSERT INTO "public"."player_items" VALUES (1899, 50, 103015, 'AK SOPMOD (10 Qty)', 10, 1);
INSERT INTO "public"."player_items" VALUES (1900, 50, 301007, 'Mini Axe (10 Qty)', 10, 1);
INSERT INTO "public"."player_items" VALUES (1901, 50, 105005, 'L115A1 (10 Qty)', 10, 1);
INSERT INTO "public"."player_items" VALUES (1902, 50, 104013, 'Kriss S.V (10 Qty)', 10, 1);
INSERT INTO "public"."player_items" VALUES (1903, 50, 2700018, 'Beret Title Shotgun', 1, 3);
INSERT INTO "public"."player_items" VALUES (1904, 50, 2700017, 'Beret Title Submachine', 1, 3);
INSERT INTO "public"."player_items" VALUES (1905, 50, 2700015, 'Beret Title Handgun', 1, 3);
INSERT INTO "public"."player_items" VALUES (1906, 50, 2700016, 'Beret Title Sniper', 1, 3);
INSERT INTO "public"."player_items" VALUES (1907, 50, 2700014, 'Beret Title Assault', 1, 3);
INSERT INTO "public"."player_items" VALUES (1909, 50, 700125, 'Panda Hat (24 hours)', 1412062044, 2);
INSERT INTO "public"."player_items" VALUES (1910, 51, 103324, 'AUG A3 Beyond (3 days)', 259200, 1);
INSERT INTO "public"."player_items" VALUES (1911, 51, 104357, 'Kriss S.V Beyond (3 days)', 259200, 1);
INSERT INTO "public"."player_items" VALUES (1912, 51, 104359, 'OA-93 Beyond (3 days)', 259200, 1);
INSERT INTO "public"."player_items" VALUES (1913, 51, 105199, 'Cheytac M200 Beyond (3 days)', 259200, 1);
INSERT INTO "public"."player_items" VALUES (1914, 51, 105200, 'AS-50 Beyond (3 days)', 259200, 1);
INSERT INTO "public"."player_items" VALUES (1915, 51, 106090, 'M1887 Beyond (3 days)', 259200, 1);
INSERT INTO "public"."player_items" VALUES (1916, 51, 315016, 'Dual Bone Knife Beyond (3 days)', 259200, 1);
INSERT INTO "public"."player_items" VALUES (1917, 51, 800357, 'Mask Beyond (3 days)', 259200, 1);
INSERT INTO "public"."player_items" VALUES (1668, 47, 103036, 'AUG A3 (10 Qty)', 9, 1);
INSERT INTO "public"."player_items" VALUES (1918, 51, 601001, 'Red Bulls', 1, 3);
INSERT INTO "public"."player_items" VALUES (1919, 51, 602002, 'Acid Pool', 1, 3);
INSERT INTO "public"."player_items" VALUES (1921, 47, 1600191, 'Increase Smoke Slot (30 days) [Active]', 1501042206, 2);
INSERT INTO "public"."player_items" VALUES (1923, 47, 1600026, 'Quick Change Weapon (30 days) [Active]', 1501042206, 2);
INSERT INTO "public"."player_items" VALUES (1925, 47, 1600027, 'Quick Reload Weapon (30 days) [Active]', 1501042206, 2);
INSERT INTO "public"."player_items" VALUES (1927, 47, 1600008, 'Ammo Up 40% (30 days) [Active]', 1501042206, 2);
INSERT INTO "public"."player_items" VALUES (1929, 47, 1600035, 'Increase Grenade Slot (30 days) [Active]', 1501042206, 2);
INSERT INTO "public"."player_items" VALUES (1931, 47, 1600028, 'HP Up 10% (30 days) [Active]', 1501042206, 2);
INSERT INTO "public"."player_items" VALUES (1933, 47, 1600080, 'Instant Respawn (30 days) [Active]', 1501042206, 2);
INSERT INTO "public"."player_items" VALUES (1935, 47, 1600078, 'Hollow Point Plus (30 days) [Active]', 1501042206, 2);
INSERT INTO "public"."player_items" VALUES (1937, 47, 1600029, 'Invisible +1 Seconds (30 days) [Active]', 1501042206, 2);
INSERT INTO "public"."player_items" VALUES (1939, 47, 1600033, 'Flashbang Protection (30 days) [Active]', 1501042206, 2);
INSERT INTO "public"."player_items" VALUES (1940, 47, 3000043, 'Aquarium (Top) Accessory (168 hours)', 1412141359, 2);

-- ----------------------------
-- Table structure for player_messages
-- ----------------------------
DROP TABLE IF EXISTS "public"."player_messages";
CREATE TABLE "public"."player_messages" (
  "object_id" int8 NOT NULL DEFAULT nextval('player_messages_seq'::regclass),
  "owner_id" int8 NOT NULL DEFAULT 0,
  "sender_id" int8 NOT NULL DEFAULT 0,
  "sender_name" varchar(255) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "clan_id" int4 NOT NULL DEFAULT 0,
  "clan_note" int4 NOT NULL DEFAULT 0,
  "text" varchar(255) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "type" int4 NOT NULL DEFAULT 0,
  "state" int4 NOT NULL DEFAULT 0,
  "expire_date" int4 NOT NULL DEFAULT 0
)
;

-- ----------------------------
-- Records of player_messages
-- ----------------------------

-- ----------------------------
-- Table structure for player_missions
-- ----------------------------
DROP TABLE IF EXISTS "public"."player_missions";
CREATE TABLE "public"."player_missions" (
  "owner_id" int8 NOT NULL DEFAULT 0,
  "current_mission" int4 NOT NULL DEFAULT 0,
  "card1" int4 NOT NULL DEFAULT 0,
  "card2" int4 NOT NULL DEFAULT 0,
  "card3" int4 NOT NULL DEFAULT 0,
  "card4" int4 NOT NULL DEFAULT 0,
  "mission1_raw" bytea NOT NULL DEFAULT '\x'::bytea,
  "mission2_raw" bytea NOT NULL DEFAULT '\x'::bytea,
  "mission3_raw" bytea NOT NULL DEFAULT '\x'::bytea,
  "mission4_raw" bytea NOT NULL DEFAULT '\x'::bytea
)
;

-- ----------------------------
-- Records of player_missions
-- ----------------------------
INSERT INTO "public"."player_missions" VALUES (48, 0, 4, 0, 0, 0, E'\\001\\001\\001\\001\\002\\002\\001\\001\\002\\001\\001\\001\\001\\001\\001\\002\\002\\001\\001\\000\\000\\000\\000\\000\\000\\000\\000\\000\\000\\000\\000\\000\\000\\000\\000\\000\\000\\000\\000\\000', E'\\\\x', E'\\\\x', E'\\\\x');
INSERT INTO "public"."player_missions" VALUES (50, 0, 0, 0, 0, 0, E'\\\\x', E'\\\\x', E'\\\\x', E'\\\\x');
INSERT INTO "public"."player_missions" VALUES (47, 0, 3, 0, 0, 0, E'\\001\\001\\001\\001\\002\\002\\001\\001\\002\\001\\001\\001\\000\\001\\000\\002\\000\\000\\000\\000\\000\\000\\000\\000\\000\\000\\000\\000\\000\\000\\000\\000\\000\\000\\000\\000\\000\\000\\000\\000', E'\\\\x', E'\\\\x', E'\\\\x');
INSERT INTO "public"."player_missions" VALUES (51, 0, 0, 0, 0, 0, E'\\\\x', E'\\\\x', E'\\\\x', E'\\\\x');

-- ----------------------------
-- Table structure for player_quickstarts
-- ----------------------------
DROP TABLE IF EXISTS "public"."player_quickstarts";
CREATE TABLE "public"."player_quickstarts" (
  "owner_id" int8 NOT NULL DEFAULT 0,
  "list0_map_id" int4 NOT NULL DEFAULT 0,
  "list0_map_rule" int4 NOT NULL DEFAULT 0,
  "list0_map_stage" int4 NOT NULL DEFAULT 0,
  "list0_map_type" int4 NOT NULL DEFAULT 0,
  "list1_map_id" int4 NOT NULL DEFAULT 0,
  "list1_map_rule" int4 NOT NULL DEFAULT 0,
  "list1_map_stage" int4 NOT NULL DEFAULT 0,
  "list1_map_type" int4 NOT NULL DEFAULT 0,
  "list2_map_id" int4 NOT NULL DEFAULT 0,
  "list2_map_rule" int4 NOT NULL DEFAULT 0,
  "list2_map_stage" int4 NOT NULL DEFAULT 0,
  "list2_map_type" int4 NOT NULL DEFAULT 0
)
;

-- ----------------------------
-- Records of player_quickstarts
-- ----------------------------
INSERT INTO "public"."player_quickstarts" VALUES (47, 1, 0, 1, 4, 13, 128, 1, 8, 51, 128, 1, 12);
INSERT INTO "public"."player_quickstarts" VALUES (48, 1, 0, 1, 4, 13, 128, 1, 8, 51, 128, 1, 12);
INSERT INTO "public"."player_quickstarts" VALUES (50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO "public"."player_quickstarts" VALUES (51, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

-- ----------------------------
-- Table structure for player_stat_acemodes
-- ----------------------------
DROP TABLE IF EXISTS "public"."player_stat_acemodes";
CREATE TABLE "public"."player_stat_acemodes" (
  "owner_id" int8 NOT NULL DEFAULT 0,
  "matches" int4 NOT NULL DEFAULT 0,
  "match_wins" int4 NOT NULL DEFAULT 0,
  "match_loses" int4 NOT NULL DEFAULT 0,
  "kills_count" int4 NOT NULL DEFAULT 0,
  "deaths_count" int4 NOT NULL DEFAULT 0,
  "headshots_count" int4 NOT NULL DEFAULT 0,
  "assists_count" int4 NOT NULL DEFAULT 0,
  "escapes_count" int4 NOT NULL DEFAULT 0,
  "winstreaks_count" int4 NOT NULL DEFAULT 0
)
;

-- ----------------------------
-- Records of player_stat_acemodes
-- ----------------------------
INSERT INTO "public"."player_stat_acemodes" VALUES (47, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO "public"."player_stat_acemodes" VALUES (48, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO "public"."player_stat_acemodes" VALUES (50, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO "public"."player_stat_acemodes" VALUES (51, 0, 0, 0, 0, 0, 0, 0, 0, 0);

-- ----------------------------
-- Table structure for player_stat_basics
-- ----------------------------
DROP TABLE IF EXISTS "public"."player_stat_basics";
CREATE TABLE "public"."player_stat_basics" (
  "owner_id" int8 NOT NULL DEFAULT 0,
  "matches" int4 NOT NULL DEFAULT 0,
  "match_wins" int4 NOT NULL DEFAULT 0,
  "match_loses" int4 NOT NULL DEFAULT 0,
  "match_draws" int4 NOT NULL DEFAULT 0,
  "kills_count" int4 NOT NULL DEFAULT 0,
  "deaths_count" int4 NOT NULL DEFAULT 0,
  "headshots_count" int4 NOT NULL DEFAULT 0,
  "assists_count" int4 NOT NULL DEFAULT 0,
  "escapes_count" int4 NOT NULL DEFAULT 0,
  "mvp_count" int4 NOT NULL DEFAULT 0,
  "total_matches" int4 NOT NULL DEFAULT 0,
  "total_kills" int4 NOT NULL DEFAULT 0
)
;

-- ----------------------------
-- Records of player_stat_basics
-- ----------------------------
INSERT INTO "public"."player_stat_basics" VALUES (50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO "public"."player_stat_basics" VALUES (51, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO "public"."player_stat_basics" VALUES (47, 6, 4, 2, 0, 24, 20, 8, 0, 0, 0, 6, 24);
INSERT INTO "public"."player_stat_basics" VALUES (48, 6, 2, 4, 0, 25, 33, 9, 0, 0, 0, 6, 25);

-- ----------------------------
-- Table structure for player_stat_battleroyales
-- ----------------------------
DROP TABLE IF EXISTS "public"."player_stat_battleroyales";
CREATE TABLE "public"."player_stat_battleroyales" (
  "owner_id" int8 NOT NULL DEFAULT 0,
  "matches" int4 NOT NULL DEFAULT 0,
  "match_wins" int4 NOT NULL DEFAULT 0,
  "match_loses" int4 NOT NULL DEFAULT 0,
  "kills_count" int4 NOT NULL DEFAULT 0,
  "deaths_count" int4 NOT NULL DEFAULT 0,
  "headshots_count" int4 NOT NULL DEFAULT 0,
  "assists_count" int4 NOT NULL DEFAULT 0,
  "escapes_count" int4 NOT NULL DEFAULT 0,
  "average_damages" int4 NOT NULL DEFAULT 0,
  "play_time" int4 NOT NULL DEFAULT 0
)
;

-- ----------------------------
-- Records of player_stat_battleroyales
-- ----------------------------
INSERT INTO "public"."player_stat_battleroyales" VALUES (47, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO "public"."player_stat_battleroyales" VALUES (48, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO "public"."player_stat_battleroyales" VALUES (50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO "public"."player_stat_battleroyales" VALUES (51, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

-- ----------------------------
-- Table structure for player_stat_clans
-- ----------------------------
DROP TABLE IF EXISTS "public"."player_stat_clans";
CREATE TABLE "public"."player_stat_clans" (
  "owner_id" int8 NOT NULL DEFAULT 0,
  "clan_matches" int4 NOT NULL DEFAULT 0,
  "clan_match_wins" int4 NOT NULL DEFAULT 0,
  "clan_match_loses" int4 NOT NULL DEFAULT 0
)
;

-- ----------------------------
-- Records of player_stat_clans
-- ----------------------------
INSERT INTO "public"."player_stat_clans" VALUES (47, 0, 0, 0);
INSERT INTO "public"."player_stat_clans" VALUES (48, 0, 0, 0);
INSERT INTO "public"."player_stat_clans" VALUES (50, 0, 0, 0);
INSERT INTO "public"."player_stat_clans" VALUES (51, 0, 0, 0);

-- ----------------------------
-- Table structure for player_stat_dailies
-- ----------------------------
DROP TABLE IF EXISTS "public"."player_stat_dailies";
CREATE TABLE "public"."player_stat_dailies" (
  "owner_id" int8 NOT NULL DEFAULT 0,
  "matches" int4 NOT NULL DEFAULT 0,
  "match_wins" int4 NOT NULL DEFAULT 0,
  "match_loses" int4 NOT NULL DEFAULT 0,
  "match_draws" int4 NOT NULL DEFAULT 0,
  "kills_count" int4 NOT NULL DEFAULT 0,
  "deaths_count" int4 NOT NULL DEFAULT 0,
  "headshots_count" int4 NOT NULL DEFAULT 0,
  "exp_gained" int4 NOT NULL DEFAULT 0,
  "point_gained" int4 NOT NULL DEFAULT 0
)
;

-- ----------------------------
-- Records of player_stat_dailies
-- ----------------------------
INSERT INTO "public"."player_stat_dailies" VALUES (50, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO "public"."player_stat_dailies" VALUES (51, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO "public"."player_stat_dailies" VALUES (48, 6, 2, 4, 0, 25, 33, 9, 2092, 53707);
INSERT INTO "public"."player_stat_dailies" VALUES (47, 6, 4, 2, 0, 24, 20, 8, 10497, 62209);

-- ----------------------------
-- Table structure for player_stat_seasons
-- ----------------------------
DROP TABLE IF EXISTS "public"."player_stat_seasons";
CREATE TABLE "public"."player_stat_seasons" (
  "owner_id" int8 NOT NULL DEFAULT 0,
  "matches" int4 NOT NULL DEFAULT 0,
  "match_wins" int4 NOT NULL DEFAULT 0,
  "match_loses" int4 NOT NULL DEFAULT 0,
  "match_draws" int4 NOT NULL DEFAULT 0,
  "kills_count" int4 NOT NULL DEFAULT 0,
  "deaths_count" int4 NOT NULL DEFAULT 0,
  "headshots_count" int4 NOT NULL DEFAULT 0,
  "assists_count" int4 NOT NULL DEFAULT 0,
  "escapes_count" int4 NOT NULL DEFAULT 0,
  "mvp_count" int4 NOT NULL DEFAULT 0,
  "total_matches" int4 NOT NULL DEFAULT 0,
  "total_kills" int4 NOT NULL DEFAULT 0
)
;

-- ----------------------------
-- Records of player_stat_seasons
-- ----------------------------
INSERT INTO "public"."player_stat_seasons" VALUES (50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO "public"."player_stat_seasons" VALUES (51, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO "public"."player_stat_seasons" VALUES (47, 5, 4, 1, 0, 21, 16, 7, 0, 0, 0, 5, 21);
INSERT INTO "public"."player_stat_seasons" VALUES (48, 5, 1, 4, 0, 21, 30, 6, 0, 0, 0, 5, 21);

-- ----------------------------
-- Table structure for player_stat_weapons
-- ----------------------------
DROP TABLE IF EXISTS "public"."player_stat_weapons";
CREATE TABLE "public"."player_stat_weapons" (
  "owner_id" int8 NOT NULL DEFAULT 0,
  "assault_rifle_kills" int4 NOT NULL DEFAULT 0,
  "assault_rifle_deaths" int4 NOT NULL DEFAULT 0,
  "sub_machine_gun_kills" int4 NOT NULL DEFAULT 0,
  "sub_machine_gun_deaths" int4 NOT NULL DEFAULT 0,
  "sniper_rifle_kills" int4 NOT NULL DEFAULT 0,
  "sniper_rifle_deaths" int4 NOT NULL DEFAULT 0,
  "machine_gun_kills" int4 NOT NULL DEFAULT 0,
  "machine_gun_deaths" int4 NOT NULL DEFAULT 0,
  "shot_gun_kills" int4 NOT NULL DEFAULT 0,
  "shot_gun_deaths" int4 NOT NULL DEFAULT 0
)
;

-- ----------------------------
-- Records of player_stat_weapons
-- ----------------------------
INSERT INTO "public"."player_stat_weapons" VALUES (50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO "public"."player_stat_weapons" VALUES (51, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO "public"."player_stat_weapons" VALUES (47, 16, 5, 9, 5, 2, 8, 0, 0, 0, 0);
INSERT INTO "public"."player_stat_weapons" VALUES (48, 11, 19, 5, 15, 8, 2, 0, 0, 0, 0);

-- ----------------------------
-- Table structure for player_titles
-- ----------------------------
DROP TABLE IF EXISTS "public"."player_titles";
CREATE TABLE "public"."player_titles" (
  "owner_id" int8 NOT NULL DEFAULT 0,
  "equip_slot1" int4 NOT NULL DEFAULT 0,
  "equip_slot2" int4 NOT NULL DEFAULT 0,
  "equip_slot3" int4 NOT NULL DEFAULT 0,
  "flags" int8 NOT NULL DEFAULT 0,
  "slots" int4 NOT NULL DEFAULT 1
)
;

-- ----------------------------
-- Records of player_titles
-- ----------------------------
INSERT INTO "public"."player_titles" VALUES (47, 14, 4, 20, 35184372088830, 3);
INSERT INTO "public"."player_titles" VALUES (48, 14, 4, 20, 35184372088830, 3);
INSERT INTO "public"."player_titles" VALUES (51, 0, 0, 0, 0, 1);
INSERT INTO "public"."player_titles" VALUES (50, 14, 4, 20, 35184372088830, 3);

-- ----------------------------
-- Table structure for player_vip
-- ----------------------------
DROP TABLE IF EXISTS "public"."player_vip";
CREATE TABLE "public"."player_vip" (
  "owner_id" int8 NOT NULL DEFAULT 0,
  "registered_ip" varchar(32) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "last_benefit" varchar(255) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "expirate" int8 NOT NULL DEFAULT 0
)
;

-- ----------------------------
-- Records of player_vip
-- ----------------------------
INSERT INTO "public"."player_vip" VALUES (48, '26.223.106.14', 'Gold', 1410302359);
INSERT INTO "public"."player_vip" VALUES (47, '26.134.207.164', 'Gold', 1410302359);

-- ----------------------------
-- Table structure for system_battlepass
-- ----------------------------
DROP TABLE IF EXISTS "public"."system_battlepass";
CREATE TABLE "public"."system_battlepass" (
  "id" int4 NOT NULL DEFAULT 0,
  "active" bool NOT NULL DEFAULT false,
  "status" int4 NOT NULL DEFAULT 0,
  "name" varchar COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "days" int4 NOT NULL DEFAULT 0,
  "start_date" int8 NOT NULL DEFAULT 0,
  "end_date" int8 NOT NULL DEFAULT 0,
  "good_id" int4 NOT NULL DEFAULT 0,
  "good_id_pa" int4 NOT NULL DEFAULT 0,
  "good_id_pb" int4 NOT NULL DEFAULT 0,
  "point_req" int4 NOT NULL DEFAULT 0
)
;

-- ----------------------------
-- Records of system_battlepass
-- ----------------------------

-- ----------------------------
-- Table structure for system_clan
-- ----------------------------
DROP TABLE IF EXISTS "public"."system_clan";
CREATE TABLE "public"."system_clan" (
  "id" int4 NOT NULL DEFAULT nextval('system_clan_seq'::regclass),
  "name" varchar(32) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "name_color" int4 NOT NULL DEFAULT 0,
  "owner_id" int8 NOT NULL DEFAULT 0,
  "rank" int4 NOT NULL DEFAULT 0,
  "exp" int4 NOT NULL DEFAULT 0,
  "point" int4 NOT NULL DEFAULT 5000,
  "logo" int8 NOT NULL DEFAULT 0,
  "info" varchar(255) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "news" varchar(255) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "create_date" int8 NOT NULL DEFAULT 0,
  "authority" int4 NOT NULL DEFAULT 0,
  "rank_limit" int4 NOT NULL DEFAULT 0,
  "min_age_limit" int4 NOT NULL DEFAULT 0,
  "max_age_limit" int4 NOT NULL DEFAULT 0,
  "matches" int4 NOT NULL DEFAULT 0,
  "match_wins" int4 NOT NULL DEFAULT 0,
  "match_loses" int4 NOT NULL DEFAULT 0,
  "max_players" int4 NOT NULL DEFAULT 50,
  "effects" int4 NOT NULL DEFAULT 0,
  "best_exp" varchar(255) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "best_participants" varchar(255) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "best_wins" varchar(255) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "best_kills" varchar(255) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "best_headshots" varchar(255) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying
)
;

-- ----------------------------
-- Records of system_clan
-- ----------------------------
INSERT INTO "public"."system_clan" VALUES (5, 'OSM-MONITOR', 0, 47, 37, 32130000, 5000, 18745347, 'HEY SUP YO~', 'Official OSM-MONITOR Clan', 20190701, 0, 0, 20, 35, 200, 125, 75, 50, 0, '42-0', '43-0', '42-0', '42-0', '43-0');

-- ----------------------------
-- Table structure for system_clan_invites
-- ----------------------------
DROP TABLE IF EXISTS "public"."system_clan_invites";
CREATE TABLE "public"."system_clan_invites" (
  "clan_id" int4 NOT NULL DEFAULT 0,
  "player_id" int8 NOT NULL DEFAULT 0,
  "invite_date" int8 NOT NULL DEFAULT 0,
  "text" varchar(255) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying
)
;

-- ----------------------------
-- Records of system_clan_invites
-- ----------------------------

-- ----------------------------
-- Table structure for system_events_login
-- ----------------------------
DROP TABLE IF EXISTS "public"."system_events_login";
CREATE TABLE "public"."system_events_login" (
  "start_date" int8 NOT NULL DEFAULT 0,
  "end_date" int8 NOT NULL DEFAULT 0,
  "goods_id" int4 NOT NULL DEFAULT 0
)
;

-- ----------------------------
-- Records of system_events_login
-- ----------------------------

-- ----------------------------
-- Table structure for system_events_mapbonus
-- ----------------------------
DROP TABLE IF EXISTS "public"."system_events_mapbonus";
CREATE TABLE "public"."system_events_mapbonus" (
  "start_date" int8 NOT NULL DEFAULT 0,
  "end_date" int8 NOT NULL DEFAULT 0,
  "map_id" int4 NOT NULL DEFAULT 0,
  "stage_type" int4 NOT NULL DEFAULT 0,
  "percent_exp" int4 NOT NULL DEFAULT 0,
  "percent_gold" int4 NOT NULL DEFAULT 0
)
;

-- ----------------------------
-- Records of system_events_mapbonus
-- ----------------------------

-- ----------------------------
-- Table structure for system_events_playtime
-- ----------------------------
DROP TABLE IF EXISTS "public"."system_events_playtime";
CREATE TABLE "public"."system_events_playtime" (
  "start_date" int8 NOT NULL DEFAULT 0,
  "end_date" int8 NOT NULL DEFAULT 0,
  "title" varchar COLLATE "pg_catalog"."default" NOT NULL DEFAULT 0,
  "playtime_req" int8 NOT NULL DEFAULT 0,
  "goods_reward1" int4 NOT NULL DEFAULT 0,
  "goods_reward2" int4 NOT NULL DEFAULT 0
)
;

-- ----------------------------
-- Records of system_events_playtime
-- ----------------------------

-- ----------------------------
-- Table structure for system_events_quest
-- ----------------------------
DROP TABLE IF EXISTS "public"."system_events_quest";
CREATE TABLE "public"."system_events_quest" (
  "start_date" int8 NOT NULL DEFAULT 0,
  "end_date" int8 NOT NULL DEFAULT 0
)
;

-- ----------------------------
-- Records of system_events_quest
-- ----------------------------

-- ----------------------------
-- Table structure for system_events_rankup
-- ----------------------------
DROP TABLE IF EXISTS "public"."system_events_rankup";
CREATE TABLE "public"."system_events_rankup" (
  "start_date" int8 NOT NULL DEFAULT 0,
  "end_date" int8 NOT NULL DEFAULT 0,
  "percent_exp" int4 NOT NULL DEFAULT 0,
  "percent_gold" int4 NOT NULL DEFAULT 0
)
;

-- ----------------------------
-- Records of system_events_rankup
-- ----------------------------

-- ----------------------------
-- Table structure for system_events_visit
-- ----------------------------
DROP TABLE IF EXISTS "public"."system_events_visit";
CREATE TABLE "public"."system_events_visit" (
  "id" int4 NOT NULL DEFAULT nextval('system_events_visit_seq'::regclass),
  "start_date" int8 NOT NULL DEFAULT 0,
  "end_date" int8 NOT NULL DEFAULT 0,
  "title" varchar(64) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "total_check" int4 NOT NULL DEFAULT 0,
  "goods_list1" varchar COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "goods_list2" varchar COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying
)
;

-- ----------------------------
-- Records of system_events_visit
-- ----------------------------
INSERT INTO "public"."system_events_visit" VALUES (1, 1409140659, 1409212359, 'ALIEN OR HALLOWEEN [P1]', 7, '10327901,20209101,10429801,10429701,10517301,40705601,70012301', '10323201,20203801,10423701,10423901,10513101,52800201,80030301');
INSERT INTO "public"."system_events_visit" VALUES (2, 1409280659, 1409052359, 'ALIEN OR HALLOWEEN [P2]', 7, '10327901,20209101,10429801,10429701,10517301,40705601,70012301', '10323201,20203801,10423701,10423901,10513101,52800201,80030301');

-- ----------------------------
-- Table structure for system_events_xmas
-- ----------------------------
DROP TABLE IF EXISTS "public"."system_events_xmas";
CREATE TABLE "public"."system_events_xmas" (
  "start_date" int8 NOT NULL DEFAULT 0,
  "end_date" int8 NOT NULL DEFAULT 0,
  "goods_id" int4 NOT NULL DEFAULT 0
)
;

-- ----------------------------
-- Records of system_events_xmas
-- ----------------------------

-- ----------------------------
-- Table structure for system_item_battlebox
-- ----------------------------
DROP TABLE IF EXISTS "public"."system_item_battlebox";
CREATE TABLE "public"."system_item_battlebox" (
  "id" int4 NOT NULL DEFAULT 0,
  "name" varchar(255) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "req_tags" int4 NOT NULL DEFAULT 0
)
;

-- ----------------------------
-- Records of system_item_battlebox
-- ----------------------------
INSERT INTO "public"."system_item_battlebox" VALUES (2800001, 'BattleBox Dragon', 1);

-- ----------------------------
-- Table structure for system_item_battlebox_awards
-- ----------------------------
DROP TABLE IF EXISTS "public"."system_item_battlebox_awards";
CREATE TABLE "public"."system_item_battlebox_awards" (
  "id" int4 NOT NULL DEFAULT 0,
  "goods_id" varchar(255) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying
)
;

-- ----------------------------
-- Records of system_item_battlebox_awards
-- ----------------------------
INSERT INTO "public"."system_item_battlebox_awards" VALUES (2800001, '10439801,10332401,10435701,10435901,10436101,10519901,10520001,10609001,31501601,80035701');

-- ----------------------------
-- Table structure for system_shop
-- ----------------------------
DROP TABLE IF EXISTS "public"."system_shop";
CREATE TABLE "public"."system_shop" (
  "item_id" int4 NOT NULL DEFAULT 0,
  "item_name" varchar(255) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "item_consume" int4 NOT NULL DEFAULT 0,
  "Item_count_list" varchar(255) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "price_cash_list" varchar(255) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "price_gold_list" varchar(255) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "title_requi" int4 NOT NULL DEFAULT 0,
  "shop_tag" int4 NOT NULL DEFAULT 0,
  "item_visible" bool NOT NULL DEFAULT true,
  "discount_percent" int4 NOT NULL DEFAULT 0
)
;

-- ----------------------------
-- Records of system_shop
-- ----------------------------
INSERT INTO "public"."system_shop" VALUES (2000280, 'Point Up 28,000', 1, '1', '280', '0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (2000320, 'Point Up 32,000', 1, '1', '320', '0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (2000360, 'Point Up 36,000', 1, '1', '360', '0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (2000400, 'Point Up 40,000', 1, '1', '400', '0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (2000600, 'Point Up 60,000', 1, '1', '600', '0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (103039, 'AK Sopmod D', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (2000100, 'Point Up 10,000', 1, '1', '100', '0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (2000200, 'Point Up 20,000', 1, '1', '200', '0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (2000220, 'Point Up 22,000', 1, '1', '220', '0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (2000180, 'Point Up 18,000', 1, '1', '180', '0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (2000140, 'Point Up 14,000', 1, '1', '140', '0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (103050, 'Fammas G2 Commando', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103051, 'Fammas G2 Scope', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103057, 'Vz52', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103102, 'HK 417', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104038, 'SCAR-L Carbine', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105011, 'SVD-Dragunov King Cobra Silver', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105017, 'Magnum L115A1 D', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105020, 'M4-SPR LV3', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105022, 'Rangemaster 762', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105034, 'DSR-1', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (106011, '870 MCS D', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (106012, 'SPAS-15 D', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (202012, 'Colt Python D', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (202023, 'IMI Uzi9', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (214006, 'Dual D. Eagle', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (700030, 'UDT-Bonnie Hat', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (700032, 'Unk 700032', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (700033, 'Unk 700033', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (700036, 'Unk 700036', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (700039, 'Tiger Hat', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (800006, 'Sport Sun Glasses', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (800005, 'M15 Gas Mask', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (800002, 'Gas Mask', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (800014, 'Mask Joker', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (800008, 'Gold PB V2 Mask', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (800024, 'Mask Green Jungle', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (800020, 'Frail Skull', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (800011, 'Skull Faceguard', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (800009, 'Wiley P. Sunglasses', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (800028, 'Jewelry Mask', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (800037, 'Unk 800037', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (700035, '707 UDT Headgear', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (2000240, 'Point Up 24,000', 1, '1', '240', '0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (1800121, 'Point Bomb Premium', 1, '1', '160', '0', 0, 1, 't', 0);
INSERT INTO "public"."system_shop" VALUES (213003, 'P99 HAK CIC D', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103045, 'M4 SR16 FC', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103052, 'Fammas M203', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103058, 'AK47 FC', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103094, 'SCAR-L Carbine', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103096, 'SCAR-L FC', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105123, 'Tactille T2', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (214005, 'Dual MK23 D', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (301011, 'Amok Kukri D', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (301014, 'Combat Machcette', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (301018, 'Ballistic Knife', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (202015, 'Taurus 454SS 8M', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (301012, 'Mini Axe D', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (315003, 'Dual Bone Knife', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (527003, 'WP Smoke Plus', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (800007, 'Skull Warmer', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (800013, 'G PB V2 Steel Mask', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (800019, 'Death Mask', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (800035, 'Smile Mask', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104170, 'Kriss S.V Couple Breaker', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 1, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103178, 'AUG A3 Couple Breaker', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 1, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105073, 'Cheytac M200 R', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (2000160, 'Point Up 16,000', 1, '1', '160', '0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (2000070, 'Point Up 7,000', 1, '1', '70', '0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (2000080, 'Point Up 8,000', 1, '1', '80', '0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (2000090, 'Point Up 9,000', 1, '1', '90', '0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (2000110, 'Point Up 11,000', 1, '1', '110', '0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (2000120, 'Point Up 12,000', 1, '1', '120', '0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (2000130, 'Point Up 13,000', 1, '1', '130', '0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (2000150, 'Point Up 15,000', 1, '1', '150', '0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (301023, 'Keris SE', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (2000300, 'Point Up 30,000', 1, '1', '300', '0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (2000500, 'Point Up 50,000', 1, '1', '500', '0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (2005000, 'Point Up 500,000', 1, '1', '5000', '0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (2010000, 'Point Up 1,000,000', 1, '1', '10000', '0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (214011, 'Dual D-Eagle GRS', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (1800052, 'Change Clan Mark', 1, '1', '600', '0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (315005, 'Bone Knife GRS', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 5, 't', 15);
INSERT INTO "public"."system_shop" VALUES (2002000, 'Point Up 200,000', 1, '1', '2000', '0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (103100, 'Fammas G2 GRS', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 5, 't', 15);
INSERT INTO "public"."system_shop" VALUES (104249, 'Kriss S.V XMAS 2015', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 2, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104251, 'OA-93 XMAS 2015', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 2, 't', 0);
INSERT INTO "public"."system_shop" VALUES (1800047, 'Change Name', 1, '1', '1200', '0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (1800051, 'Change Clan Name', 1, '1', '1200', '0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (1800085, 'PB Inspector', 1, '100', '1200', '0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (1800708, 'PBIC 2015 Gacha', 1, '1', '900', '0', 0, 0, 't', 1);
INSERT INTO "public"."system_shop" VALUES (2000005, 'Point Up 500', 1, '1', '5', '0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (2000007, 'Point Up 700', 1, '1', '7', '0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (2000010, 'Point Up 1,000', 1, '1', '10', '0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (2000015, 'Point Up 1,500', 1, '1', '15', '0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (2000020, 'Point Up 2,000', 1, '1', '20', '0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (2000030, 'Point Up 3,000', 1, '1', '30', '0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (2000050, 'Point Up 5,000', 1, '1', '50', '0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (2000060, 'Point Up 6,000', 1, '1', '60', '0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (103241, 'AUG A3 XMAS 2015', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 2, 't', 0);
INSERT INTO "public"."system_shop" VALUES (1800120, 'Point Bomb Basic', 1, '1', '120', '0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104083, 'Kriss S.V GRS', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 5, 't', 15);
INSERT INTO "public"."system_shop" VALUES (700149, 'GRS Headgear', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (106026, 'M1887 GRS', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 5, 't', 15);
INSERT INTO "public"."system_shop" VALUES (103242, 'SC-2010 XMAS 2015', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 2, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105139, 'Tactille T2 XMAS 2015', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 2, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105147, 'Cheytac M200 Cupid', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104005, 'Kriss S.V Cupid', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (106065, 'M1887 Cupid', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104017, 'P90 Ext. Cupid', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103276, 'SC-2010 Dolphin', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104289, 'Kriss S.V Dolphin', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (106072, 'M1887 Dolphin', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104291, 'P90 Ext. Dolphin', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103098, 'M4A1 GRS', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 5, 't', 15);
INSERT INTO "public"."system_shop" VALUES (103250, 'AUG A3 Cupid', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (2001000, 'Point Up 100,000', 1, '1', '1000', '0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (103275, 'AUG A3 Dolphin', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105138, 'Cheytac M200 XMAS 2015', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 2, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105068, 'XM-2010', 1, '100', '0', '70000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103097, 'AUG A3 GRS', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 5, 't', 15);
INSERT INTO "public"."system_shop" VALUES (106047, 'Cerberus Shotgun', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103246, 'AK-12', 1, '100', '0', '89000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103268, 'Pindad SS2 V5', 1, '100', '0', '67000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (301066, 'Death Scythe', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104046, 'UMP45 Dual Mag D', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (700137, 'Santa Hat', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (700140, 'Coboy Hat', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (701106, 'Super Headgear', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (2700005, 'Cross Star Beret', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (2700007, 'Red Star Beret', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (2700011, 'Yellow Star Beret', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103284, 'Groza', 1, '100', '0', '90000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104001, 'MP5K Ext.', 1, '100', '0', '23000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104002, 'Spectre Ext.', 1, '100', '0', '25000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104003, 'K-1 Ext.', 1, '100', '0', '20000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104004, 'MP7 Ext.', 1, '100', '0', '20000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103001, 'SG 550 Ext.', 1, '100', '0', '30000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103002, 'AK-47 Ext.', 1, '100', '0', '35000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103003, 'M4A1 Ext.', 1, '100', '0', '30000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103005, 'F2000 Ext.', 1, '100', '0', '28000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103014, 'SG 550 S.', 1, '100', '0', '45000', 8, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103036, 'AUG A3 Ext.', 1, '100', '0', '42000', 12, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103015, 'AK SOPMOD', 1, '100', '0', '75000', 12, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (2700018, 'Master Blaster Beret', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 39, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (103116, 'F2000 Reload', 1, '100', '0', '41000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103117, 'SG 550 Reload', 1, '100', '0', '41000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103118, 'SS2-V Para Sniper Reload', 1, '100', '0', '46000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103153, 'SC-2010', 1, '100', '0', '90000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103223, 'M14 EBR', 1, '100', '0', '70000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103236, 'K2C', 1, '100', '0', '70000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104008, 'UMP46 Ext.', 1, '100', '0', '25000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104033, 'AKS74U Ext.', 1, '100', '0', '45000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104096, 'AKS74U Reload', 1, '100', '0', '47000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104097, 'UMP45 Reload', 1, '100', '0', '33000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104098, 'Spectre Reload', 1, '100', '0', '33000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104100, 'MP5K Reload', 1, '100', '0', '32000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104134, 'OA-93', 1, '100', '0', '42000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105001, 'Dragunov', 1, '100', '0', '50000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105002, 'PSG1', 1, '100', '0', '45000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105004, 'SS-69 S.', 1, '100', '0', '40000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105007, 'PSG1 S.', 1, '100', '0', '65000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105029, 'VSK94', 1, '100', '0', '55000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105035, 'SVU', 1, '100', '0', '45000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (106020, 'Kel-Tec KSG-15', 1, '100', '0', '55000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (106043, 'Remington ETA', 1, '100', '0', '70000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (202001, 'Desert Eagle', 1, '100', '0', '15000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (202011, 'Glock 18', 1, '100', '0', '65000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (202035, 'MK.23 Reload', 1, '100', '0', '30000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (202036, 'Desert Eagle Reload', 1, '100', '0', '30000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (202094, 'U22 Neos', 1, '100', '0', '32500', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (323003, 'Brass Knuckle', 1, '100', '0', '30000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (508002, 'FlashBang', 1, '100', '0', '26000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (301021, 'Keris', 1, '100', '0', '28000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (106015, '870MCS D', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (106042, 'Zombie Slayer', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103035, 'SG550 Camo D', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (214007, 'Dual HK45', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (800010, 'PB 6200 Mask', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (800034, 'Rorschach Mask', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105099, 'Cheytac M200 Couple Breaker', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 1, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104052, 'Kriss S.V Batik', 1, '100', '0', '42000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105030, 'Cheytac M200', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (202007, 'Colt Python', 1, '100', '0', '55000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103174, 'XM8', 1, '100', '0', '67000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (110011, 'Ultimax 100', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (202002, 'MK 23 Ext.', 1, '100', '0', '15000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (110001, 'MK 46 Ext.', 1, '100', '0', '28000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (213007, 'P99-Hak CIC Reload', 1, '100', '0', '30000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105059, 'HK-PSG1 Reload', 1, '100', '0', '45000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103009, 'AK-47 Gold', 1, '100', '0', '55000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105012, 'SVD-Dragunov King Cobra Gold', 1, '100', '0', '95000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105052, 'Cheytac M200 GRS', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 5, 't', 15);
INSERT INTO "public"."system_shop" VALUES (105169, 'Magnum L115A1 Dolphin', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105168, 'Cheytac M200 Dolphin', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103053, 'Pindad SS2 V4', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (800001, 'Boeing Sunglasses', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (800012, 'Standard Faceguard', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105028, 'SCD Dragunnov D', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (315008, 'Dual Kunai', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (202019, 'MK23 Silencer D', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (527001, 'WP Smoke', 1, '100', '0', '38000', 42, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (106010, 'M1887 W.', 1, '100', '0', '80000', 39, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103013, 'G36C Ext.', 1, '100', '0', '40000', 10, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105006, 'Dragunov SE', 1, '100', '0', '70000', 14, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105005, 'Magnum L115A1', 1, '100', '0', '65000', 18, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (2700016, 'Secret Assasinator Beret', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 19, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (104011, 'P90 Ext.', 1, '100', '0', '45000', 22, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104013, 'Kriss S.V', 1, '100', '0', '50000', 24, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (2700017, 'Tricky Intruder Beret', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 25, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (315001, 'Dual Knife', 1, '100', '0', '26000', 26, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (301004, 'Amok Kukri', 1, '100', '0', '30000', 28, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (301007, 'Mini Axe', 1, '100', '0', '30000', 29, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (213001, 'P99&Hak', 1, '100', '0', '30000', 30, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (214001, 'Dual Handgun', 1, '100', '0', '35000', 32, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (214002, 'Dual D-Eagle', 1, '100', '0', '35000', 32, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (106004, '870MCS W.', 1, '100', '0', '30000', 35, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (106003, 'SPAS-15', 1, '100', '0', '70000', 37, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (407002, 'C-5', 1, '100', '0', '44000', 40, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103231, 'AUG A3 Steam', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (104235, 'Kriss S.V Steam', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (105130, 'Cheytac M200 Steam', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (106061, 'M1887 Steam', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (103040, 'AUG A3 D', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104045, 'MP7 Dot Sight D', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103038, 'G36C D', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (106017, 'M1887 D', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103175, 'SCAR-L Carbine D', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (407012, 'Decoy Bomb', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103176, 'SCAR-L Recon D', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104231, 'MP9 Ext D', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (106046, 'UTS-15 D', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104032, 'P90MC D', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (234005, 'Compound Blow Black', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105164, 'AS-50 D', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104029, 'P90 Ext D', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (407018, 'M14 Mine D', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105098, 'DSR-1 D', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (301041, 'Arabian Sword', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (118005, 'Dual Uzi Mini', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (234004, 'Compound Blow Blue', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (527002, 'WP Smoke D', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (234001, 'Compound Blow', 1, '100', '0', '38500', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (118006, 'Dual Uzi Micro', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (118004, 'Dual Uzi', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (301103, 'Bamboo Runcing', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (301058, 'Chinese Cleaver', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (301053, 'Frying Pan', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (301071, 'Goat Hammer', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (301147, 'Karambit', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (301120, 'Monkey Hammer', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (407005, 'Chocolate Grenade', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (407016, 'Football Bomb', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (407008, 'Ketupat Grenade', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (407050, 'Water Bomb', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (407026, 'Meat Bomb', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (528011, 'Medical Kit Pigeon', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (528001, 'Medical Kit', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (528005, 'Medical Kit Opor Ayam', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (301064, 'Badmintor Racket', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (110003, 'RPD', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (323005, 'Pumpkin Knuckle', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104047, 'Kriss S.V Batik D', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (701104, 'Target Headgear', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (700125, 'Panda Hat', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (202053, 'MGL 32', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (315002, 'Dual Knife D', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104138, 'P90 Ext Brazuca', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (202024, 'Kriss Vector SDP', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103254, 'Watergun 2016', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104031, 'Kriss S.V D', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (106051, 'Songkran Watergun', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104193, 'EVO 3', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103092, 'AK SOPMOD R', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105083, 'Cheytac M200 Brazuca', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (106037, 'M1887 Brazuca', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (103144, 'AUG A3 R', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (407003, 'C-5 D', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104123, 'Kriss S.V R', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (301024, 'Candy Cane', 1, '100,200', '0,0', '31000,62000', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (106034, 'M1887 W R', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (301104, 'Keris PBIC 2015', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (103115, 'AN-94', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103274, 'AUG A3 Demonic', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (202070, 'R.B 454 SS8M+S PBIC 2015', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (103143, 'FG 42', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103172, 'Snow Ball Blaster', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103123, 'TAR-21', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103166, 'SC-2010 D', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105167, 'Cheytac M200 Demonic', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (213008, 'Colt Python Cutlass', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104025, 'Spectre W D', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104288, 'P90 Ext Demonic', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (104159, 'OA-93 D', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103232, 'AUG A3 Halloween', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (105021, 'Rangemaster 338', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105016, 'PSG-1 D', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (202038, 'HK45 Halloween', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (202021, 'Glock-18 D', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (110004, 'L86 LSW', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104237, 'Kriss S.V Halloween', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (800303, 'Mask Halloween', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (104239, 'P90 Ext Halloween', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (105131, 'Cheytac M200 Halloween', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (528002, 'Medical Kit Halloween', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (104286, 'Kriss S.V Demonic', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (800258, 'Brazuca Mask', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (800299, 'Mask PBIC 2015 Premium', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (103155, 'AUG A3 Brazuca', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (301052, 'Fang Blade Brazuca', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (202049, 'Colt Python Brazuca', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (104139, 'Kriss S.V Brazuca ', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (104216, 'Kriss S.V PBIC 2015', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (105032, 'Barret M82A1', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104218, 'P90 Ext PBIC 2015', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (106058, 'M1887 PBIC 2015', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (105120, 'Cheytac M200 PBIC 2015', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (1800131, 'FaceBook Box', 1, '1', '950', '0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103270, 'Pindad SS2 V5 Silver', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (110002, 'MK46 Silver', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103029, 'G36C Silver', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103021, 'M4A1 Silver', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (323004, 'Silver Knuckle', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (213002, 'P99-Hak CIC Silver', 1, '100', '0', '12000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103032, 'F2000 Silver', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104022, 'MP7 Silver', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103019, 'SG550 Silver', 1, '100', '0', '28000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104014, 'MP5K Silver', 1, '100', '0', '23000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104020, 'Spectre Silver', 1, '100', '0', '23000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (202095, 'U22 Neos Silver', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (202027, 'MK23 Silver', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (106006, 'SPAS-15 Silver', 1, '100', '0', '26000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105008, 'SSG-69 Silver', 1, '100', '0', '20000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (213004, 'P99-Hak CIC Silver D', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (118011, 'Dual Uzi Micro Silencer Silver', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (234002, 'Compound Bow Silver', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103056, 'SG550 Silver D', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (106016, 'SPAS-15 Silver D', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105009, 'HK-PSG1 Silver', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (106014, '870MCS Silver D', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104023, 'UMP45 Silver', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104042, 'Spectre Silver D', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105027, 'SSG-69 Silver D', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105010, 'SVD Dragunov Silver', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (106008, '870MCS Silver', 1, '100', '0', '24000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104099, 'Pindad SS1 R5 Relaod', 1, '100', '0', '31000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105060, 'Rangemaster 338. Reload', 1, '100', '0', '33000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (2700015, 'Excellent Sharpshooter Beret', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 34, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (105061, 'Rangemaster 762. Reload', 1, '100', '0', '34000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103145, 'FG42 Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103168, 'HK-417 Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105072, 'Walther WA2000', 1, '100', '0', '15000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105095, 'DSR-1 Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104043, 'Pindad SS1 R5', 1, '100', '0', '36000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103017, 'AK-47 Silver', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103286, 'Groza Silver', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104041, 'MP5K Silver D', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (202017, 'Colt Python Gold', 1, '100', '0', '12000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104219, 'P90 MC Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103167, 'AN-94 Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103064, 'Fammas G2 Commando Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103285, 'Groza Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103239, 'M14 EBR Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103023, 'M4A1 Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103061, 'Pindad SS2 V4 Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103269, 'Pindad SS2 V5 Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103170, 'SCAR-L Carbine Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103154, 'SC-2010 Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103177, 'XM8 Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (234003, 'Compound Bow Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (214004, 'Dual Dessert Eagle Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104026, 'Kriss S.V Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104136, 'OA-93 Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (110012, 'Ultimax-100 Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (301043, 'Combat Machette Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (301009, 'M-7 Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (202018, 'Colt Python Gold D', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (202071, 'Glock-18 Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (202096, 'U22-Neos Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104194, 'EVO-3 Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104049, 'Pindad SS1 R5 Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (106059, 'M1887 Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (106048, 'UTS-15 Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103247, 'AK-12 Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105159, 'AS-50 Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105014, 'SVD-Dragunov Gold D', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105094, 'SVU Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105093, 'VSK-94 Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (407007, 'K-413 Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103054, 'AK-47 Gold D', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (700107, 'Angel Ring', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (800257, 'Midnight Mask', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 5, 't', 10);
INSERT INTO "public"."system_shop" VALUES (103150, 'Tar-21 Midnight', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 5, 't', 10);
INSERT INTO "public"."system_shop" VALUES (301082, 'Arabian Sword Midnight', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 5, 't', 10);
INSERT INTO "public"."system_shop" VALUES (104132, 'Kriss S.V Midnight', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 5, 't', 10);
INSERT INTO "public"."system_shop" VALUES (800271, 'Midnight Non Logo Mask', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 5, 't', 10);
INSERT INTO "public"."system_shop" VALUES (103162, 'Tar-21 Midnight Non Logo', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 5, 't', 10);
INSERT INTO "public"."system_shop" VALUES (104149, 'Kriss S.V Midnight Non Logo', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 5, 't', 10);
INSERT INTO "public"."system_shop" VALUES (103221, 'AK-47 SOPMOD Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103037, 'AUG A3 Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103237, 'K2C Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103169, 'TAR-21 Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (214018, 'Scorpion Vz61 Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (202008, 'Dessert Eagle Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104161, 'MP-9 Ext. Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104075, 'P90 Ext Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104162, 'PP-19 Bizon Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (106044, 'Remington ETA Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105121, 'Cheytac M200 Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105015, 'Magnum L115A1 Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105146, 'PGM-Hecate2 Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105132, 'Tactilite T2 Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105074, 'Walther WA2000 Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103010, 'M4A1 Ext SE', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (700139, 'Bandana Hat', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (700158, 'Toy Hat', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (700195, 'Monkey Mask', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (700038, 'Pumpkin Mask', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (701103, 'Reinforce Headgear', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (701105, 'Headgear Strengthen Plus', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105036, 'Magnum L115A1 SE', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105054, 'SVU SE', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (2700004, 'Bope Beret', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (2700006, 'Kopassus Beret', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (2700009, 'Super Beret', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (2700003, 'Black PB Beret', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (700048, 'SWAT Helmet', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (700147, 'Keen Eyes Egg Mask', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (700133, 'Tarantula Egg Mask', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (700148, 'Acid Poll Egg Mask', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (700134, 'Red Bulls Egg Mask', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104359, 'OA-93 Beyond', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104361, 'P90 Ext Beyond', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (315016, 'Dual Bone Knife Beyond', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (800357, 'Mask Beyond', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103062, 'Fammas G2 Commando E-Sport', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (103063, 'AUG-A3 E-Sport', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (105033, 'Magnum L115A1 E-Sport', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (301065, 'Keris E-Sport', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (2700014, 'Ferocious Beast Beret', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 13, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (103324, 'AUG A3 Beyond', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105199, 'Cheytac M200 Beyond', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105200, 'AS-50 Beyond', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (106090, 'M1887 Beyond', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105024, 'HK-PSG1 Gold', 1, '100', '0', '37000', 16, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104007, 'MP5K Gold', 1, '100', '0', '24000', 20, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (800261, 'Mask PBIC 2014', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (106038, 'M1887 PBIC 2014', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (104009, 'Spectre SE', 1, '100', '0', '27000', 20, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104357, 'Kriss S.V Beyond', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103120, 'AUG A3 PBIC 2013', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (103228, 'AUG A3 Camo Soldier', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103229, 'G36C Camo Soldier', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (315010, 'Dual Bone Knife Camo Soldier', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105128, 'Range Master 338 Camo Soldier', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104027, 'P90 MC Camo', 1, '100', '0', '44000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103122, 'AK-47 PBIC 2013', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (103121, 'M4A1 PBIC 2013', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (104103, 'Kriss S.V PBIC 2013', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (800216, 'Mask PBIC 2013', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (103193, 'AUG A3 Lion Flame', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (105108, 'Cheytac M200 Lion Flame', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (103159, 'AUG A3 PBIC 2014', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (104146, 'P90 Ext PBIC 2014', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (105087, 'Cheytac M200 PBIC 2014', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (202074, 'Kriss V. SDP Camo Soldier', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104234, 'P90 MC Camo Soldier', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (202083, 'Colt Python Beast', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (106067, 'M1887 Beast', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (104050, 'Kriss S.V E-Sport', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (104157, 'Kriss S.V Toy', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (105092, 'Magnum L115A1 Toy', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (301129, 'Keris Beast', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (602224, 'Hitman Acid Poll', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (703010, 'Cangacerio Hat', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (103165, 'AUG A3 Toy', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (202052, 'Colt Python Toy', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (103256, 'AUG A3 Beast', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (104258, 'Kriss S.V Beast', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (105151, 'Cheytac M200 Beast', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (103279, 'AUG A3 Alien', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (301149, 'Fang Blade Alien', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (202091, 'MK23 Alien', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (104298, 'Kriss S.V Alien', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (104297, 'P90 Ext Alien', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (105173, 'Cheytac M200 Alien', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (105174, 'Magnum L115A1 Alien', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (407056, 'K-400 Alien', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (700123, 'Mask Alien - Blue Eye', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (700124, 'Mask Alien - Red Eye', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (601328, 'High School Rica', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (602328, 'High School Chou', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (601286, 'General Viper Red', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (602287, 'General Hide', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103114, 'M4A1 Elite', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (315015, 'Ancient Thai Sword', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (601122, 'Ninja Rica', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (602210, 'Ninja Chou', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (601010, 'Rica', 1, '100', '0', '32000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (301017, 'Fang Blade', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (602011, 'Chou', 1, '100', '0', '32000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (601069, 'Caiman Grey', 1, '100', '0', '28000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (602070, 'Wolf', 1, '100', '0', '28000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (301047, 'Keris SE XMAS', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (110010, 'L86LSW XMAS', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (601244, 'Vacance 2017 Tarantula', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (602156, 'Vacance 2017 Keen Eyes', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (601136, 'Hitman Red Bulls', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (601149, 'Hitman D-Fox', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (602237, 'Hitman Leopard', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (601089, 'Vacance 2017 Viper Red', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (601397, 'Halloween Nurse Tarantula', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (602400, 'Halloween Nurse Keen Eyes', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (602177, 'Vacance 2017 Hide', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (202026, 'HK69', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (202022, 'Colt 45', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (803015, 'Alien Mask', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (106018, 'SPAS-15 MSC', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (301146, 'Death Scythe Demonic', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (800323, 'Demonic Mask', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (1800049, 'Reset Score', 1, '1', '1200', '0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (1800048, 'Reset History', 1, '1', '900', '0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (1800050, 'Reset Escape', 1, '1', '900', '0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (1800053, 'Reset Clan History', 1, '1', '1400', '0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (1800055, 'Clan Member +50', 1, '1', '1500', '0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (1800056, 'Clear Clan Point', 1, '1', '900', '0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (1801145, 'Tag Bomb', 1, '1', '5000', '0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (202029, 'GL 06', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (2800001, 'Dragon BattleBox', 1, '1', '2000', '0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104398, 'Dragon', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (214017, 'Scorpion Vz61', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (3400016, 'Aurora Namecard', 2, '604800', '570', '0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (3400147, '11th Anniversary Namecard', 2, '604800', '570', '0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (3200187, 'COVID-19 Spray', 2, '604800', '570', '0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (3000022, 'Rust Iron (Top) Accesory', 2, '604800', '570', '0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (3200075, 'Dolphin Spray', 2, '604800', '570', '0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (3200179, 'X-MAS 2019 Spray', 2, '604800', '570', '0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (3000023, 'Rust Iron (Side) Accesory', 2, '604800', '570', '0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (3000024, 'Rust Iron (Front) Accesory', 2, '604800', '570', '0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (3000025, 'Wraith (Top) Accessory', 2, '604800', '570', '0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (3400153, 'Adana Namecard', 2, '604800', '570', '0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (3400154, 'Ankara Namecard', 2, '604800', '570', '0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (3400155, 'Antalya Namecard', 2, '604800', '570', '0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (3400174, 'Aquarius Namecard', 2, '604800', '570', '0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (3400047, 'Firefly Namecard', 2, '604800', '570', '0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (3400159, 'Egypt Namecard', 2, '604800', '570', '0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (3000026, 'Wraith (Side) Accessory', 2, '604800', '570', '0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (3000027, 'Wraith (Front) Accessory', 2, '604800', '570', '0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (3000043, 'Aquarium (Top) Accessory', 2, '604800', '570', '0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (3000044, 'Aquarium (Side) Accessory', 2, '604800', '570', '0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (3000045, 'Aquarium (Front) Accessory', 2, '604800', '570', '0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (601668, 'Natasha Rein', 2, '86400,259200,604800,2592000', '200,470,700,3200', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (601678, 'Natasha Superstar', 2, '86400,259200,604800,2592000', '200,470,700,3200', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (601666, 'Natasha', 2, '86400,259200,604800,2592000', '200,470,700,3200', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (602667, 'Queen', 2, '86400,259200,604800,2592000', '200,470,700,3200', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (602669, 'Queen Rein', 2, '86400,259200,604800,2592000', '200,470,700,3200', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (602679, 'Queen Superboxer', 2, '86400,259200,604800,2592000', '200,470,700,3200', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (664657, 'Dajeong Office CT', 2, '86400,259200,604800,2592000', '200,470,700,3200', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (632660, 'Gwoon Office FR', 2, '86400,259200,604800,2592000', '200,470,700,3200', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (632656, 'Dajeong Office FR', 2, '86400,259200,604800,2592000', '200,470,700,3200', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (664661, 'Gwoon Office CT', 2, '86400,259200,604800,2592000', '200,470,700,3200', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (702267, 'Pony Hair FR', 2, '86400,259200,604800,2592000', '200,470,700,3200', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (702255, 'Bob Hair', 2, '86400,259200,604800,2592000', '200,470,700,3200', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (702260, 'Dajeong Hair', 2, '86400,259200,604800,2592000', '200,470,700,3200', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (702262, 'Gwoon Hair', 2, '86400,259200,604800,2592000', '200,470,700,3200', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (702268, 'Pony Hair CT', 2, '86400,259200,604800,2592000', '200,470,700,3200', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (702285, 'Shot Brown', 2, '86400,259200,604800,2592000', '200,470,700,3200', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (702286, 'Shot Black', 2, '86400,259200,604800,2592000', '200,470,700,3200', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (118013, 'Dual Uzi Mini Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (301280, 'GH5007 SignaturePB', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105468, 'KAR98 Songhran', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105377, 'BORA12 Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 2, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105375, 'BORA12', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105376, 'BORA12 Silver', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (301286, 'Kukrii Shark', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (301362, 'butterfly Songkran', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103711, 'Aug A3 Golden Phoenix', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 2, 't', 0);
INSERT INTO "public"."system_shop" VALUES (702283, 'Afro Rainbow Wig', 1, '100', '40000', '0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (702284, 'Afro Black Wig', 1, '100', '40000', '0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (700258, 'Ear Set Blue Wig', 2, '86400,259200,604800,2592000', '200,470,700,3200', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (116003, 'RPG7 Boss', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (116001, 'RPG7 Outpost', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (103712, 'SC-2010 Golden Phoenix', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 2, 't', 0);
INSERT INTO "public"."system_shop" VALUES (106231, 'ZombieSlayer Golden Phoenix', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 2, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105485, 'Cheytac Golden Phoenix', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 2, 't', 0);
INSERT INTO "public"."system_shop" VALUES (116002, 'RPG7', 2, '86400,259200', '100000,300000', '0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (136049, 'Kriss Golden Phoenix', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 2, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (301211, 'Aitor Oso Negro Faca NOVA', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 2, 't', 0);
INSERT INTO "public"."system_shop" VALUES (301274, 'Pig Hammer', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (301276, 'TrenchKnife', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (601003, 'Tarantula', 1, '100', '0', '20000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (602004, 'Keen Eyes', 1, '100', '0', '20000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (601005, 'Viper Red', 1, '100', '0', '20000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (602009, 'Hide', 1, '100', '0', '20000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (601007, 'D-Fox', 1, '100', '0', '20000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (602008, 'Leopard', 1, '100', '0', '20000', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103518, 'AK-SOPMOD Zepetto', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103519, 'FAMAS G2 Zepetto', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103520, 'M4A1 Zepetto', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (301264, 'Arabian Sword Zepetto', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (301265, 'FangBlade Zepetto', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (301266, 'Fangblade Zepetto Speed', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (301267, 'Karambit Zepetto', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (301268, 'Keris Zepetto', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (700257, 'Ear Set Red Wig', 2, '86400,259200,604800,2592000', '200,470,700,3200', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (702270, 'Pony Hair Blue', 2, '86400,259200,604800,2592000', '200,470,700,3200', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (702269, 'Pny Hair Brown', 2, '86400,259200,604800,2592000', '200,470,700,3200', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (702254, 'Updo Hair', 2, '86400,259200,604800,2592000', '200,470,700,3200', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (702256, 'Wave Hair', 2, '86400,259200,604800,2592000', '200,470,700,3200', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (702282, 'Bob Ash Gray', 2, '86400,259200,604800,2592000', '200,470,700,3200', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (702281, 'Bob Purple', 2, '86400,259200,604800,2592000', '200,470,700,3200', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (702289, 'Long Brown Wave', 2, '86400,259200,604800,2592000', '200,470,700,3200', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (702287, 'Long Green Bang', 2, '86400,259200,604800,2592000', '200,470,700,3200', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (702288, 'Long Purple Pny Tail', 2, '86400,259200,604800,2592000', '200,470,700,3200', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103219, 'AUG A3 PBIC 2015', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 'f', 0);
INSERT INTO "public"."system_shop" VALUES (118009, 'Dual Uzi Micro Silencer', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 0, 't', 0);
INSERT INTO "public"."system_shop" VALUES (301269, 'M-9 Zepetto', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103527, 'AUG A3 GunZeeD', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103528, 'SC 2010 GunZeeD', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105361, 'Cheytac M200 GunZeeD', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105362, 'Tactilite T2 GunZeeD', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (301270, 'GH5007 GunZeeD', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103541, 'AUG-A3 SignaturePB', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103542, 'SC 2010 SignaturePB', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105372, 'Tactilite T2 SignaturePB', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105431, 'KAR98 DarkCupid', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103585, 'Aug Halloween 2019', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105406, 'Cheytac halloween 2019', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105407, 'Tactitle Halloween 2019', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (301302, 'machete halloween 2019', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103537, 'AUG A3 Lovely Valentine', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 2, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103538, 'SC 2010 Lovely Valentine', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 2, 't', 0);
INSERT INTO "public"."system_shop" VALUES (234009, 'Compound Bow Lovely Valentine', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 2, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105369, 'Cheytac M200 Lovely Valentine', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 2, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103549, 'MPT 76', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103550, 'MPT 76 Silver', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103551, 'MPT 76 Gold', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103576, 'AUG A3 Crimson', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104817, 'KrissSuperV Crimson', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104819, 'OA93 Crimson', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105398, 'Cheytac M200 Crimson', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (301298, 'GH5007 Crimson', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103575, 'AUG A3 PinkyPig', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104813, 'KrissSuperV PinkyPig', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104815, 'OA93 PinkyPig', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105397, 'Cheytac M200 PinkyPig', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (301297, 'GH5007 PinkyPig', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104796, 'KrissSuperV Ramadan19-Deluxe', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104798, 'P90 MC Ramadan19-Deluxe', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104799, 'APC9 Ramadan19-Deluxe', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (105391, 'Tactilite T2 Ramadan19-Deluxe', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (301293, 'FangBlade Ramadan19-Deluxe', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (103643, 'M4 Azure Dragon', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104897, 'Kriss S.V CRB', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (104969, 'Kriss S.V CRB Crow', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 4, 't', 0);
INSERT INTO "public"."system_shop" VALUES (301363, 'Light Saber Knife', 2, '86400,259200,604800,2592000', '100,270,500,2700', '0,0,0,0', 0, 2, 't', 0);

-- ----------------------------
-- Table structure for system_shop_effects
-- ----------------------------
DROP TABLE IF EXISTS "public"."system_shop_effects";
CREATE TABLE "public"."system_shop_effects" (
  "coupon_id" int4 NOT NULL DEFAULT 0,
  "coupon_name" varchar(64) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "coupon_count_day_list" varchar(255) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "price_cash_list" varchar(255) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "price_gold_list" varchar(255) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "shop_tag" int4 NOT NULL DEFAULT 0,
  "coupon_visible" bool NOT NULL DEFAULT true,
  "discount_percent" int4 NOT NULL DEFAULT 0
)
;

-- ----------------------------
-- Records of system_shop_effects
-- ----------------------------
INSERT INTO "public"."system_shop_effects" VALUES (1700026, 'Quick Change Weapon', '1,3,7,30', '100,270,500,1500', '0,0,0,0', 0, 't', 0);
INSERT INTO "public"."system_shop_effects" VALUES (1700027, 'Quick Reload Weapon', '1,3,7,30', '100,270,500,1500', '0,0,0,0', 0, 't', 0);
INSERT INTO "public"."system_shop_effects" VALUES (1700080, 'Instant Respawn', '1,3,7,30', '100,270,500,1500', '0,0,0,0', 0, 't', 0);
INSERT INTO "public"."system_shop_effects" VALUES (1700032, 'Hollow Point', '1,3,7,30', '100,270,500,1500', '0,0,0,0', 0, 't', 0);
INSERT INTO "public"."system_shop_effects" VALUES (1700187, 'Change Muzzle Color', '1,3,7,30', '100,270,500,1500', '0,0,0,0', 0, 't', 0);
INSERT INTO "public"."system_shop_effects" VALUES (1700035, 'Increase Grenade Slot', '1,3,7,30', '100,270,500,1500', '0,0,0,0', 0, 't', 0);
INSERT INTO "public"."system_shop_effects" VALUES (1700036, 'Jack Hollow Point', '1,3,7,30', '100,270,500,1500', '0,0,0,0', 0, 't', 0);
INSERT INTO "public"."system_shop_effects" VALUES (1700191, 'Increase Smoke Slot', '1,3,7,30', '100,270,500,1500', '0,0,0,0', 0, 't', 0);
INSERT INTO "public"."system_shop_effects" VALUES (1700078, 'Hollow Point Plus', '1,3,7,30', '100,270,500,1500', '0,0,0,0', 0, 't', 0);
INSERT INTO "public"."system_shop_effects" VALUES (1700014, 'Change Crosshair Color', '1,3,7,30', '100,270,500,1500', '0,0,0,0', 0, 't', 0);
INSERT INTO "public"."system_shop_effects" VALUES (1700006, 'Change Nick Color', '1,3,7,30', '100,270,500,1500', '0,0,0,0', 0, 't', 0);
INSERT INTO "public"."system_shop_effects" VALUES (1700064, 'Quick Respawn 50%', '1,3,7,30', '100,270,500,1500', '0,0,0,0', 0, 't', 0);
INSERT INTO "public"."system_shop_effects" VALUES (1700077, 'Quick Respawn 20%', '1,3,7,30', '100,270,500,1500', '0,0,0,0', 0, 't', 0);
INSERT INTO "public"."system_shop_effects" VALUES (1700011, 'Free Move, Free Pass', '1,3,7,30', '100,270,500,1500', '0,0,0,0', 0, 't', 0);
INSERT INTO "public"."system_shop_effects" VALUES (1700017, 'Get Dropped Weapon', '1,3,7,30', '100,270,500,1500', '0,0,0,0', 0, 't', 0);
INSERT INTO "public"."system_shop_effects" VALUES (1700079, 'Defense Up 20%', '1,3,7,30', '100,270,500,1500', '0,0,0,0', 0, 't', 0);
INSERT INTO "public"."system_shop_effects" VALUES (1700029, 'Invisible +1 Seconds', '1,3,7,30', '100,270,500,1500', '0,0,0,0', 0, 't', 0);
INSERT INTO "public"."system_shop_effects" VALUES (1700033, 'Flashbang Protection', '1,3,7,30', '100,270,500,1500', '0,0,0,0', 0, 't', 0);
INSERT INTO "public"."system_shop_effects" VALUES (1700034, 'C-4 Speed Up', '1,3,7,30', '100,270,500,1500', '0,0,0,0', 0, 't', 0);
INSERT INTO "public"."system_shop_effects" VALUES (1700031, 'Full Metal Jack', '1,3,7,30', '100,270,500,1500', '0,0,0,0', 0, 't', 0);
INSERT INTO "public"."system_shop_effects" VALUES (1700185, 'Ammo Up 10%', '1,3,7,30', '100,270,500,1500', '0,0,0,0', 0, 't', 0);
INSERT INTO "public"."system_shop_effects" VALUES (1700008, 'Ammo Up 40%', '1,3,7,30', '100,270,500,1500', '0,0,0,0', 0, 't', 0);
INSERT INTO "public"."system_shop_effects" VALUES (1700009, 'Fake Rank', '1,3,7,30', '100,270,500,1500', '0,0,0,0', 0, 't', 0);
INSERT INTO "public"."system_shop_effects" VALUES (1700010, 'Fake Nick', '1,3,7,30', '100,270,500,1500', '0,0,0,0', 0, 't', 0);
INSERT INTO "public"."system_shop_effects" VALUES (1700012, 'EXP Clan 150%', '1,3,7,30', '100,270,500,1500', '0,0,0,0', 0, 't', 0);
INSERT INTO "public"."system_shop_effects" VALUES (1700119, 'EXP Up 150%', '1,3,7,30', '100,270,500,1500', '0,0,0,0', 0, 't', 0);
INSERT INTO "public"."system_shop_effects" VALUES (1700044, 'Defense Up 10%', '1,3,7,30', '100,270,500,1500', '0,0,0,0', 0, 't', 0);
INSERT INTO "public"."system_shop_effects" VALUES (1700040, 'HP Up 5%', '1,3,7,30', '100,270,500,1500', '0,0,0,0', 0, 't', 0);
INSERT INTO "public"."system_shop_effects" VALUES (1700030, 'Defense Up 5%', '1,3,7,30', '100,270,500,1500', '0,0,0,0', 0, 't', 0);
INSERT INTO "public"."system_shop_effects" VALUES (1700028, 'HP Up 10%', '1,3,7,30', '100,270,500,1500', '0,0,0,0', 0, 't', 0);
INSERT INTO "public"."system_shop_effects" VALUES (1700065, 'Vest 90% (GM)', '1,3,7,30', '100,270,500,1500', '0,0,0,0', 4, 't', 0);
INSERT INTO "public"."system_shop_effects" VALUES (1700186, 'Fake Nick (Event)', '14', '540', '0', 3, 't', 0);

-- ----------------------------
-- Table structure for system_shop_repair
-- ----------------------------
DROP TABLE IF EXISTS "public"."system_shop_repair";
CREATE TABLE "public"."system_shop_repair" (
  "item_id" int4 NOT NULL DEFAULT 0,
  "price_cash" int4 NOT NULL DEFAULT 0,
  "price_gold" int4 NOT NULL DEFAULT 0,
  "quantity" int4 NOT NULL DEFAULT 0,
  "repairable" bool NOT NULL DEFAULT false
)
;

-- ----------------------------
-- Records of system_shop_repair
-- ----------------------------
INSERT INTO "public"."system_shop_repair" VALUES (301007, 0, 13000, 100, 't');
INSERT INTO "public"."system_shop_repair" VALUES (301004, 0, 13000, 100, 't');
INSERT INTO "public"."system_shop_repair" VALUES (315001, 0, 13000, 100, 't');
INSERT INTO "public"."system_shop_repair" VALUES (104013, 0, 24000, 100, 't');
INSERT INTO "public"."system_shop_repair" VALUES (104011, 0, 24000, 100, 't');
INSERT INTO "public"."system_shop_repair" VALUES (104007, 0, 24000, 100, 't');
INSERT INTO "public"."system_shop_repair" VALUES (104009, 0, 24000, 100, 't');
INSERT INTO "public"."system_shop_repair" VALUES (105005, 0, 23000, 100, 't');
INSERT INTO "public"."system_shop_repair" VALUES (105024, 0, 23000, 100, 't');
INSERT INTO "public"."system_shop_repair" VALUES (105006, 0, 23000, 100, 't');
INSERT INTO "public"."system_shop_repair" VALUES (103015, 0, 22000, 100, 't');
INSERT INTO "public"."system_shop_repair" VALUES (103036, 0, 22000, 100, 't');
INSERT INTO "public"."system_shop_repair" VALUES (103013, 0, 22000, 100, 't');
INSERT INTO "public"."system_shop_repair" VALUES (103014, 0, 22000, 100, 't');
INSERT INTO "public"."system_shop_repair" VALUES (527001, 0, 12000, 100, 't');
INSERT INTO "public"."system_shop_repair" VALUES (407002, 0, 12000, 100, 't');
INSERT INTO "public"."system_shop_repair" VALUES (106010, 0, 22000, 100, 't');
INSERT INTO "public"."system_shop_repair" VALUES (106003, 0, 22000, 100, 't');
INSERT INTO "public"."system_shop_repair" VALUES (106004, 0, 22000, 100, 't');
INSERT INTO "public"."system_shop_repair" VALUES (214001, 0, 15000, 100, 't');
INSERT INTO "public"."system_shop_repair" VALUES (214002, 0, 15000, 100, 't');
INSERT INTO "public"."system_shop_repair" VALUES (213001, 0, 15000, 100, 't');
INSERT INTO "public"."system_shop_repair" VALUES (702283, 40000, 0, 100, 't');
INSERT INTO "public"."system_shop_repair" VALUES (702284, 40000, 0, 100, 't');

-- ----------------------------
-- Table structure for system_shop_sets
-- ----------------------------
DROP TABLE IF EXISTS "public"."system_shop_sets";
CREATE TABLE "public"."system_shop_sets" (
  "id" int4 NOT NULL DEFAULT 0,
  "name" varchar(32) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "visible" bool NOT NULL DEFAULT true
)
;

-- ----------------------------
-- Records of system_shop_sets
-- ----------------------------

-- ----------------------------
-- Table structure for system_shop_sets_items
-- ----------------------------
DROP TABLE IF EXISTS "public"."system_shop_sets_items";
CREATE TABLE "public"."system_shop_sets_items" (
  "set_id" int4 NOT NULL DEFAULT 0,
  "set_name" varchar(32) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "id" int4 NOT NULL DEFAULT 0,
  "name" varchar(32) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "consume" int4 NOT NULL DEFAULT 0,
  "count" int4 NOT NULL DEFAULT 0,
  "price_cash" int4 NOT NULL DEFAULT 0,
  "price_gold" int4 NOT NULL DEFAULT 0
)
;

-- ----------------------------
-- Records of system_shop_sets_items
-- ----------------------------

-- ----------------------------
-- Alter sequences owned by
-- ----------------------------
SELECT setval('"public"."accounts_seq"', 52, true);

-- ----------------------------
-- Alter sequences owned by
-- ----------------------------
SELECT setval('"public"."base_auto_ban_seq"', 8, false);

-- ----------------------------
-- Alter sequences owned by
-- ----------------------------
SELECT setval('"public"."base_ban_history_seq"', 8, false);

-- ----------------------------
-- Alter sequences owned by
-- ----------------------------
SELECT setval('"public"."base_nick_history_seq"', 14, true);

-- ----------------------------
-- Alter sequences owned by
-- ----------------------------
SELECT setval('"public"."base_redeem_history_seq"', 12, true);

-- ----------------------------
-- Alter sequences owned by
-- ----------------------------
SELECT setval('"public"."player_characters_seq"', 208, true);

-- ----------------------------
-- Alter sequences owned by
-- ----------------------------
SELECT setval('"public"."player_friends_seq"', 27, true);

-- ----------------------------
-- Alter sequences owned by
-- ----------------------------
SELECT setval('"public"."player_items_seq"', 1941, true);

-- ----------------------------
-- Alter sequences owned by
-- ----------------------------
SELECT setval('"public"."player_messages_seq"', 30, true);

-- ----------------------------
-- Alter sequences owned by
-- ----------------------------
SELECT setval('"public"."system_clan_seq"', 9, true);

-- ----------------------------
-- Alter sequences owned by
-- ----------------------------
SELECT setval('"public"."system_events_visit_seq"', 6, false);

-- ----------------------------
-- Primary Key structure for table accounts
-- ----------------------------
ALTER TABLE "public"."accounts" ADD CONSTRAINT "accounts_pkey" PRIMARY KEY ("player_id");

-- ----------------------------
-- Primary Key structure for table base_auto_ban
-- ----------------------------
ALTER TABLE "public"."base_auto_ban" ADD CONSTRAINT "base_auto_ban_pkey" PRIMARY KEY ("object_id");

-- ----------------------------
-- Primary Key structure for table base_ban_history
-- ----------------------------
ALTER TABLE "public"."base_ban_history" ADD CONSTRAINT "base_ban_history_pkey" PRIMARY KEY ("object_id");

-- ----------------------------
-- Primary Key structure for table base_nick_history
-- ----------------------------
ALTER TABLE "public"."base_nick_history" ADD CONSTRAINT "base_nick_history_pkey" PRIMARY KEY ("object_id");

-- ----------------------------
-- Primary Key structure for table base_redeem_history
-- ----------------------------
ALTER TABLE "public"."base_redeem_history" ADD CONSTRAINT "base_redeem_history_pkey" PRIMARY KEY ("object_id");

-- ----------------------------
-- Primary Key structure for table player_battlepass
-- ----------------------------
ALTER TABLE "public"."player_battlepass" ADD CONSTRAINT "player_battlepass_pkey" PRIMARY KEY ("owner_id");

-- ----------------------------
-- Primary Key structure for table player_characters
-- ----------------------------
ALTER TABLE "public"."player_characters" ADD CONSTRAINT "player_characters_pkey" PRIMARY KEY ("object_id");

-- ----------------------------
-- Primary Key structure for table player_configs
-- ----------------------------
ALTER TABLE "public"."player_configs" ADD CONSTRAINT "player_configs_pkey" PRIMARY KEY ("owner_id");

-- ----------------------------
-- Primary Key structure for table player_equipments
-- ----------------------------
ALTER TABLE "public"."player_equipments" ADD CONSTRAINT "player_equipments_pkey" PRIMARY KEY ("owner_id");

-- ----------------------------
-- Primary Key structure for table player_events
-- ----------------------------
ALTER TABLE "public"."player_events" ADD CONSTRAINT "player_events_pkey" PRIMARY KEY ("owner_id");

-- ----------------------------
-- Primary Key structure for table player_friends
-- ----------------------------
ALTER TABLE "public"."player_friends" ADD CONSTRAINT "player_friends_pkey" PRIMARY KEY ("object_id");

-- ----------------------------
-- Primary Key structure for table player_items
-- ----------------------------
ALTER TABLE "public"."player_items" ADD CONSTRAINT "player_items_pkey" PRIMARY KEY ("object_id");

-- ----------------------------
-- Primary Key structure for table player_messages
-- ----------------------------
ALTER TABLE "public"."player_messages" ADD CONSTRAINT "player_messages_pkey" PRIMARY KEY ("object_id");

-- ----------------------------
-- Primary Key structure for table player_missions
-- ----------------------------
ALTER TABLE "public"."player_missions" ADD CONSTRAINT "player_missions_pkey" PRIMARY KEY ("owner_id");

-- ----------------------------
-- Primary Key structure for table player_quickstarts
-- ----------------------------
ALTER TABLE "public"."player_quickstarts" ADD CONSTRAINT "player_quickstarts_pkey" PRIMARY KEY ("owner_id");

-- ----------------------------
-- Primary Key structure for table player_stat_acemodes
-- ----------------------------
ALTER TABLE "public"."player_stat_acemodes" ADD CONSTRAINT "player_stat_acemodes_pkey" PRIMARY KEY ("owner_id");

-- ----------------------------
-- Primary Key structure for table player_stat_basics
-- ----------------------------
ALTER TABLE "public"."player_stat_basics" ADD CONSTRAINT "player_stats_pkey" PRIMARY KEY ("owner_id");

-- ----------------------------
-- Primary Key structure for table player_stat_battleroyales
-- ----------------------------
ALTER TABLE "public"."player_stat_battleroyales" ADD CONSTRAINT "player_stat_battleroyale_pkey" PRIMARY KEY ("owner_id");

-- ----------------------------
-- Primary Key structure for table player_stat_clans
-- ----------------------------
ALTER TABLE "public"."player_stat_clans" ADD CONSTRAINT "player_stat_clan_pkey" PRIMARY KEY ("owner_id");

-- ----------------------------
-- Primary Key structure for table player_stat_dailies
-- ----------------------------
ALTER TABLE "public"."player_stat_dailies" ADD CONSTRAINT "player_stat_dailies_pkey" PRIMARY KEY ("owner_id");

-- ----------------------------
-- Primary Key structure for table player_stat_seasons
-- ----------------------------
ALTER TABLE "public"."player_stat_seasons" ADD CONSTRAINT "player_stat_seasons_pkey" PRIMARY KEY ("owner_id");

-- ----------------------------
-- Primary Key structure for table player_stat_weapons
-- ----------------------------
ALTER TABLE "public"."player_stat_weapons" ADD CONSTRAINT "player_stat_weapons_pkey" PRIMARY KEY ("owner_id");

-- ----------------------------
-- Primary Key structure for table player_titles
-- ----------------------------
ALTER TABLE "public"."player_titles" ADD CONSTRAINT "player_titles_pkey" PRIMARY KEY ("owner_id");

-- ----------------------------
-- Primary Key structure for table player_vip
-- ----------------------------
ALTER TABLE "public"."player_vip" ADD CONSTRAINT "player_vip_pkey" PRIMARY KEY ("owner_id");

-- ----------------------------
-- Primary Key structure for table system_battlepass
-- ----------------------------
ALTER TABLE "public"."system_battlepass" ADD CONSTRAINT "system_battle_pass_pkey" PRIMARY KEY ("id");

-- ----------------------------
-- Primary Key structure for table system_clan
-- ----------------------------
ALTER TABLE "public"."system_clan" ADD CONSTRAINT "system_clan_pkey" PRIMARY KEY ("id");

-- ----------------------------
-- Primary Key structure for table system_clan_invites
-- ----------------------------
ALTER TABLE "public"."system_clan_invites" ADD CONSTRAINT "system_clan_invites_pkey" PRIMARY KEY ("clan_id");

-- ----------------------------
-- Primary Key structure for table system_events_login
-- ----------------------------
ALTER TABLE "public"."system_events_login" ADD CONSTRAINT "system_events_login_pkey" PRIMARY KEY ("goods_id");

-- ----------------------------
-- Primary Key structure for table system_events_mapbonus
-- ----------------------------
ALTER TABLE "public"."system_events_mapbonus" ADD CONSTRAINT "system_events_mapbonus_pkey" PRIMARY KEY ("map_id");

-- ----------------------------
-- Primary Key structure for table system_events_playtime
-- ----------------------------
ALTER TABLE "public"."system_events_playtime" ADD CONSTRAINT "system_events_playtime_pkey" PRIMARY KEY ("goods_reward1", "goods_reward2");

-- ----------------------------
-- Primary Key structure for table system_events_visit
-- ----------------------------
ALTER TABLE "public"."system_events_visit" ADD CONSTRAINT "system_events_visit_pkey" PRIMARY KEY ("id");

-- ----------------------------
-- Primary Key structure for table system_events_xmas
-- ----------------------------
ALTER TABLE "public"."system_events_xmas" ADD CONSTRAINT "system_events_xmas_pkey" PRIMARY KEY ("goods_id");

-- ----------------------------
-- Primary Key structure for table system_item_battlebox
-- ----------------------------
ALTER TABLE "public"."system_item_battlebox" ADD CONSTRAINT "system_item_battlebox_pkey" PRIMARY KEY ("id");

-- ----------------------------
-- Primary Key structure for table system_item_battlebox_awards
-- ----------------------------
ALTER TABLE "public"."system_item_battlebox_awards" ADD CONSTRAINT "system_item_battlebox_awards_pkey" PRIMARY KEY ("id");

-- ----------------------------
-- Primary Key structure for table system_shop
-- ----------------------------
ALTER TABLE "public"."system_shop" ADD CONSTRAINT "system_shop_pkey" PRIMARY KEY ("item_id");

-- ----------------------------
-- Primary Key structure for table system_shop_effects
-- ----------------------------
ALTER TABLE "public"."system_shop_effects" ADD CONSTRAINT "system_shop_effects_pkey" PRIMARY KEY ("coupon_id");

-- ----------------------------
-- Primary Key structure for table system_shop_repair
-- ----------------------------
ALTER TABLE "public"."system_shop_repair" ADD CONSTRAINT "system_shop_repair_pkey" PRIMARY KEY ("item_id");

-- ----------------------------
-- Primary Key structure for table system_shop_sets
-- ----------------------------
ALTER TABLE "public"."system_shop_sets" ADD CONSTRAINT "system_shop_sets_pkey" PRIMARY KEY ("id");

-- ----------------------------
-- Primary Key structure for table system_shop_sets_items
-- ----------------------------
ALTER TABLE "public"."system_shop_sets_items" ADD CONSTRAINT "system_shop_sets_items_pkey" PRIMARY KEY ("id");
