--
-- PostgreSQL database dump
--

-- Dumped from database version 13.16
-- Dumped by pg_dump version 16.4

-- Started on 2024-10-26 08:53:48

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 5 (class 2615 OID 2200)
-- Name: public; Type: SCHEMA; Schema: -; Owner: postgres
--

-- *not* creating schema, since initdb creates it


ALTER SCHEMA public OWNER TO postgres;

--
-- TOC entry 2 (class 3079 OID 16384)
-- Name: adminpack; Type: EXTENSION; Schema: -; Owner: -
--

CREATE EXTENSION IF NOT EXISTS adminpack WITH SCHEMA pg_catalog;


--
-- TOC entry 3015 (class 0 OID 0)
-- Dependencies: 2
-- Name: EXTENSION adminpack; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION adminpack IS 'administrative functions for PostgreSQL';


SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 203 (class 1259 OID 16404)
-- Name: category; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.category (
    category_id integer NOT NULL,
    title character varying(20) NOT NULL
);


ALTER TABLE public.category OWNER TO postgres;

--
-- TOC entry 202 (class 1259 OID 16399)
-- Name: company; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.company (
    company_id integer NOT NULL,
    name character varying(20) NOT NULL
);


ALTER TABLE public.company OWNER TO postgres;

--
-- TOC entry 201 (class 1259 OID 16394)
-- Name: items; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.items (
    item_id integer NOT NULL,
    title character varying(20) NOT NULL,
    company_id integer NOT NULL,
    category_id integer NOT NULL
);


ALTER TABLE public.items OWNER TO postgres;

--
-- TOC entry 205 (class 1259 OID 16424)
-- Name: items/order; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."items/order" (
    item_id integer NOT NULL,
    order_id integer NOT NULL,
    item_order_id integer NOT NULL
);


ALTER TABLE public."items/order" OWNER TO postgres;

--
-- TOC entry 204 (class 1259 OID 16419)
-- Name: order; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."order" (
    order_id integer NOT NULL,
    name character varying(50) NOT NULL,
    "time" time without time zone NOT NULL,
    shipping_adress character varying(100)
);


ALTER TABLE public."order" OWNER TO postgres;

--
-- TOC entry 2870 (class 2606 OID 16408)
-- Name: category category_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.category
    ADD CONSTRAINT category_pkey PRIMARY KEY (category_id);


--
-- TOC entry 2868 (class 2606 OID 16403)
-- Name: company company_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.company
    ADD CONSTRAINT company_pkey PRIMARY KEY (company_id);


--
-- TOC entry 2874 (class 2606 OID 16428)
-- Name: items/order items/order_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."items/order"
    ADD CONSTRAINT "items/order_pkey" PRIMARY KEY (item_order_id);


--
-- TOC entry 2866 (class 2606 OID 16398)
-- Name: items items_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.items
    ADD CONSTRAINT items_pkey PRIMARY KEY (item_id);


--
-- TOC entry 2872 (class 2606 OID 16423)
-- Name: order order_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."order"
    ADD CONSTRAINT order_pkey PRIMARY KEY (order_id);


--
-- TOC entry 2877 (class 2606 OID 16434)
-- Name: items/order 2item; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."items/order"
    ADD CONSTRAINT "2item" FOREIGN KEY (item_id) REFERENCES public.items(item_id) NOT VALID;


--
-- TOC entry 2878 (class 2606 OID 16429)
-- Name: items/order 2order; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."items/order"
    ADD CONSTRAINT "2order" FOREIGN KEY (order_id) REFERENCES public."order"(order_id) NOT VALID;


--
-- TOC entry 2875 (class 2606 OID 16414)
-- Name: items item2cat; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.items
    ADD CONSTRAINT item2cat FOREIGN KEY (category_id) REFERENCES public.category(category_id) NOT VALID;


--
-- TOC entry 2876 (class 2606 OID 16409)
-- Name: items item2comp; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.items
    ADD CONSTRAINT item2comp FOREIGN KEY (company_id) REFERENCES public.company(company_id) NOT VALID;


--
-- TOC entry 3014 (class 0 OID 0)
-- Dependencies: 5
-- Name: SCHEMA public; Type: ACL; Schema: -; Owner: postgres
--

REVOKE USAGE ON SCHEMA public FROM PUBLIC;
GRANT ALL ON SCHEMA public TO PUBLIC;


-- Completed on 2024-10-26 08:53:49

--
-- PostgreSQL database dump complete
--

