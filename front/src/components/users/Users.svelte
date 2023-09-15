<script lang="ts">
	let notes: User[] = [];

	function getUsers() {
		fetch('/api/users')
			.then((response) => response.json())
			.then((data: User[]) => {
				notes = data;
			});
	}

	import { onMount } from 'svelte';

	onMount(async () => {
		getUsers();
	});
</script>

{#each notes as note}
	<p>
		<b> {note.login}, {note.password} </b>
	</p>
{/each}
